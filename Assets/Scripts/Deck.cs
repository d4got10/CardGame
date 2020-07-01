using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CardGame
{
    public enum GameSuitsCount
    {
        one = 1,
        two = 2,
        four = 4
    }

    public class Deck : MonoBehaviour
    {

        [SerializeField] private CardFactoryObject _cardFactory;

        [SerializeField] private GameRuler _gameRuler;

        [SerializeField] private int _deckSize;

        [SerializeField] private GameSuitsCount _type;

        [SerializeField] private float _timeDelay = 0.04f;

        [SerializeField] private PlaceHolder[] _holders;

        public List<Card> Cards { get; private set; }

        public bool HasStartedGame { get; private set; } = false;

        public UnityEvent OnHasStartedGame;

        public void StartGame(int suitsCount)
        {
            StartCoroutine(StartGameCoroutine(suitsCount));
        }

        IEnumerator StartGameCoroutine(int suitsCount)
        {
            _type = (GameSuitsCount)suitsCount;
            CreateDeck();
            var setup = _gameRuler.GetPlaceHoldersCardCountSetup();

            int sum = 0;
            foreach (int a in setup)
                sum += a;

            while(sum > 0)
                for(int i = 0; i < setup.Length; i++)
                {
                    if(setup[i] > 0)
                    {
                        sum--;
                        setup[i]--;
                        Card card = Cards[0];
                        if (setup[i] == 0)
                            card.Open();
                        _holders[i].ForcedStack(card);
                        Cards.RemoveAt(0);
                        yield return new WaitForSeconds(_timeDelay);
                    }
                }

            yield return new WaitForSeconds(1);
            OnHasStartedGame.Invoke();
            HasStartedGame = true;

            if(Cards.Count == 0)
            {
                Destroy(gameObject);
            }
        }

        private void CreateDeck()
        {
            Cards = new List<Card>();
            for (int suit = 1; suit <= _deckSize; suit++)
            {
                for (int value = 1; value <= 13; value++)
                {
                    Cards.Add(CreateCard(new Vector3(0, 0, 0.1f), (Suits)(suit % (int)_type + 1), (Values)value, true));
                    Cards = Shuffle(Cards);
                }
            }
        }

        public Card CreateCard(Vector3 position, Suits suit, Values value, bool hidden)
        {
            GameObject card = _cardFactory.CreateCard(suit, value, hidden);
            var cardComponent = card.GetComponent<Card>();

            card.transform.parent = transform;
            card.transform.localPosition = position;

            return cardComponent;
        }

        public virtual void OnClick()
        {
            AddCards();
        }

        public virtual void AddCards()
        {
            if (!HasStartedGame) return;

            bool eachIsNotEmpty = true;
            for (int i = 0; i < _holders.Length; i++)
                if (_holders[i].StackedCard == null) eachIsNotEmpty = false;

            if (eachIsNotEmpty)
            {
                for (int i = 0; i < _holders.Length; i++)
                {
                    if (Cards.Count > 0)
                    {
                        Card card = Cards[0];
                        card.Open();
                        _holders[i].ForcedStack(card);
                        Cards.RemoveAt(0);
                    }
                }
            }

            if (Cards.Count == 0) Destroy(gameObject);
        }

        private void OnMouseDown()
        {
            OnClick();
        }

        private List<Card> Shuffle(List<Card> deck)
        {
            List<Card> shuffledDeck = new List<Card>();
            int size = deck.Count;
            for (int i = 0; i < size; i++)
            {
                Card card = deck[Random.Range(0, deck.Count)];
                shuffledDeck.Add(card);
                deck.Remove(card);
            }
            return shuffledDeck;
        }
    }
}
