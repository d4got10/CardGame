using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CardGame
{
    public class PlaceHolder : MonoBehaviour, IStackable
    {
        public float StackedCardYOffset = 0;

        public Card.BoolFunctionDelegate CanBeGrabbedFunction;
        public Card.CanBeStackedFunctionDelegate CanBeStackedFunction;

        public Card StackedCard { get; protected set; }
        public Action OnBecameEmpty;

        private void Awake()
        {
            GameRuler gameRuler = FindObjectOfType<GameRuler>();

            CanBeGrabbedFunction = gameRuler.CanBeGrabbedFunction;
            CanBeStackedFunction = gameRuler.CanBeStackedFunction;
        }

        public bool CanBeGrabbed()
        {
            return false;
        }

        public Vector3 GetStackedWorldPosition()
        {
            return transform.position;
        }

        public virtual bool CanStack(Card card)
        {
            return true;
        }

        public void ForcedStack(Card card)
        {
            if (StackedCard == null)
            {
                card.Parent = this;

                card.PickUp();

                BoundChild(card);
                StackedCard.MoveTo(new Vector3(0, 0f, -0.1f));          
                card.PutDown();

                StackedCard.OnChangedParent?.Invoke(this);
            }
            else
            {
                StackedCard.ForcedStack(card);
            }

        }

        public bool Stack(Card card)
        {
            if (StackedCard == null)
            {
                if (CanBeStackedFunction(this, card))
                {
                    card.Parent = this;

                    BoundChild(card);
                    StackedCard.transform.localPosition = new Vector3(0, 0f, -0.1f);

                    return true;
                }
            }

            return false;
        }

        protected virtual void BoundChild(Card card)
        {
            StackedCard = card;

            StackedCard.CanBeGrabbedFunction = CanBeGrabbedFunction;
            StackedCard.CanBeStackedFunction = CanBeStackedFunction;

            StackedCard.transform.parent = transform;
            StackedCard.OnChangedParent += UnStack;
            StackedCard.IsWinnable = (StackedCard.Value == Values.King);
            StackedCard.StackedCardYOffset = StackedCardYOffset;
            GetComponent<BoxCollider2D>().enabled = false;
        }

        public virtual void UnStack(IStackable newParent)
        {
            if ((object)newParent != this)
            {
                StackedCard.OnChangedParent -= UnStack;
                StackedCard = null;

                GetComponent<BoxCollider2D>().enabled = true;
                OnBecameEmpty?.Invoke();
            }
        }
    }
}