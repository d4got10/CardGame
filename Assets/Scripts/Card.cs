using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CardGame
{
    public class Card : MonoBehaviour, IStackable
    {
        public static Action OnAnyCardChangedPlace;
        public static Action OnAnyCardStacked;

        public float StackedCardYOffset;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite _backSprite;
        [SerializeField] private BoxCollider2D _collider;

        public Suits Suit { get; private set; }
        public Values Value { get; private set; }
        public bool Hidden;

        public Vector3 Position { get; private set; }

        public bool IsWinnable = false;
        public Action OnWin;

        public IStackable Parent { get; set; }
        public Card StackedCard { get; private set; }

        public GameRuler GameRuler { get; set; }

        public delegate bool BoolFunctionDelegate();
        public BoolFunctionDelegate CanBeGrabbedFunction;

        public delegate bool CanBeStackedFunctionDelegate(Card parent, Card child);
        public CanBeStackedFunctionDelegate CanBeStackedFunction;


        public Action<IStackable> OnChangedParent;
        public Action OnPickedUp;
        public Action OnPutDown;

        private bool _pickedUp = false;
        private float _mouseYOffset;
        private Sprite _faceSprite;

        private void Awake() => OnChangedParent += (_) => { OnAnyCardChangedPlace?.Invoke(); };

        public void Set(Suits suit, Values value, bool hidden)
        {
            Suit = suit;
            Value = value;
            Hidden = hidden;

            _faceSprite = _spriteRenderer.sprite;
            if (Hidden) _spriteRenderer.sprite = _backSprite;
        }

        public bool CanBeGrabbed()
        {
            return CanBeGrabbedFunction();
        }

        public Vector3 GetStackedWorldPosition()
        {
            return transform.parent.position + Position - new Vector3(0, StackedCardYOffset, 0);
        }

        private void Update()
        {
            if (_pickedUp) //Перенос карты, выбранной мышкой
            {
                Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                newPosition.y += _mouseYOffset;
                newPosition.z = -5f; //CONSTANT
                transform.position = newPosition;
            }
        }

        public void Open() //Открывание карты лицо к верху
        {
            _spriteRenderer.sprite = _faceSprite;
            Hidden = false;
            if (Value == Values.King) IsWinnable = true;
        }

        public void Win()
        {
            if (Value == Values.King)
            {
                OnChangedParent?.Invoke(null);
                Destroy(gameObject);
            }

            OnWin?.Invoke();
        }

        public void MoveTo(Vector3 position)
        {
            Position = position;
            StartCoroutine(MoveToCoroutine(position));
        }

        public void MoveToGhostly(Vector3 position)
        {
            var stackedCardPosition = position;
            stackedCardPosition.z = -9;
            StartCoroutine(MoveToGhostlyCoroutine(stackedCardPosition));
        }

        public void ForcedStack(Card card) //Процедура вынужденной привязки ребенка(card) к родителю(this)
        {
            if (StackedCard == null)
            {
                card.PickUp();

                BoundChild(card);

                StackedCard.MoveTo(new Vector3(0, -StackedCardYOffset, -0.1f));

                if ((!Hidden && IsWinnable && StackedCard.Suit == Suit && card.Value == Value - 1) || StackedCard.Value == Values.King)
                    StackedCard.IsWinnable = true;
                else
                    StackedCard.IsWinnable = false;

                if (IsWinnable && Value == Values.Ace) Win();

                card.PutDown();
                StackedCard.OnChangedParent?.Invoke(this);
            }
            else
            {
                StackedCard.ForcedStack(card);
            }

        }

        public bool Stack(Card card) //Процедура привязки ребенка(card) к родителю(this)
        {
            if (CanBeStackedFunction(this,card))
            {
                BoundChild(card);

                StackedCard.transform.localPosition = new Vector3(0, -StackedCardYOffset, -0.1f);

                if ((!Hidden && IsWinnable && StackedCard.Suit == Suit) || StackedCard.Value == Values.King)
                    StackedCard.IsWinnable = true;
                else
                    StackedCard.IsWinnable = false;              

                OnAnyCardStacked?.Invoke();

                return true;
            }

            return false;
        }

        private void BoundChild(Card card)
        {
            card.Parent = this;

            card.StackedCardYOffset = StackedCardYOffset;
            card.CanBeGrabbedFunction = CanBeGrabbedFunction;
            card.CanBeStackedFunction = CanBeStackedFunction;

            StackedCard = card;
            StackedCard.transform.parent = transform;

            StackedCard.OnChangedParent += UnStack;
            StackedCard.OnWin += Win;

            OnPickedUp += StackedCard.PickUp;
            OnPutDown += StackedCard.PutDown;
        }

        public void UnStack(IStackable newParent) //Процедура отвязки ребенка от родителя и наоборот
        {
            if ((object)newParent != this)
            {
                OnPickedUp -= StackedCard.PickUp;
                OnPutDown -= StackedCard.PutDown;

                StackedCard.OnChangedParent -= UnStack;
                StackedCard.OnWin -= Win;
                StackedCard = null;

                if (Hidden) Open();
            }
        }

        public void PickUp() //Вызывается когда родитель был поднят
        {
            OnPickedUp?.Invoke();
            GetComponent<BoxCollider2D>().enabled = false;
        }

        public void PutDown() //Вызывается когда родитель был опущен
        {
            if (StackedCard != null) StackedCard.IsWinnable = IsWinnable;
            if (IsWinnable && Value == Values.Ace) Win();
            OnPutDown?.Invoke();
            GetComponent<BoxCollider2D>().enabled = true;
        }

        public void OnMouseDown() //Событие нажатия на карту
        {
            if (CanBeGrabbed())
            {
                _mouseYOffset = (transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)).y;
                _pickedUp = true;
                OnPickedUp?.Invoke();
            }
        }

        public void OnMouseUp() //Событие отпускания карты
        {
            if (_pickedUp)
            {
                GetPlace();
                _pickedUp = false;
                if (StackedCard != null) StackedCard.IsWinnable = IsWinnable;
                if (IsWinnable && Value == Values.Ace) Win();
                OnPutDown?.Invoke();
            }
        }

        private void GetPlace() //Процедура определения новой позиции карты либо оставление ее на прежнем месте
        {
            ContactFilter2D filter = new ContactFilter2D();
            filter.NoFilter();
            List<Collider2D> colliders = new List<Collider2D>();
            Physics2D.OverlapBox(transform.position, transform.localScale, 0, filter, colliders);

            float distance = 10f;
            IStackable stackable = null;

            foreach (var collider in colliders)
            {
                if (collider.transform != transform)
                {
                    if (distance > (collider.transform.position - transform.position).magnitude)
                    {
                        if (collider.TryGetComponent(out IStackable stackableTemp))
                            stackable = stackableTemp;

                        distance = (collider.transform.position - transform.position).magnitude;
                    }
                }
            }

            if (stackable != null)
            {
                if (stackable.Stack(this))
                {
                    OnChangedParent?.Invoke(stackable);
                    if (Value == Values.Ace && IsWinnable) Win();
                    Position = transform.localPosition;
                    return;
                }
            }

            transform.localPosition = Position;
        }

        IEnumerator MoveToCoroutine(Vector3 position)
        {
            _collider.enabled = false;
            while ((transform.localPosition - position).magnitude > 0.02f && !_pickedUp)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, position, 0.1f);
                yield return null;
            }
            transform.localPosition = Position;
            _collider.enabled = true;
        }

        IEnumerator MoveToGhostlyCoroutine(Vector3 position)
        {
            _collider.enabled = false;
            while ((transform.position - position).magnitude > 0.02f && !_pickedUp)
            {
                transform.position = Vector3.Lerp(transform.position, position, 0.1f);
                yield return null;
            }
            transform.localPosition = Position;
            _collider.enabled = true;
        }
    }

    public enum Suits
    {
        Clubs = 1,
        Diamonds,
        Hearts,
        Spades,
    }

    public enum Values
    {
        Ace = 1,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King
    }
}
