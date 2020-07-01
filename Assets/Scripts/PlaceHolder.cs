using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CardGame
{
    public abstract class PlaceHolder : MonoBehaviour, IStackable
    {
        public float StackedCardYOffset = 0;

        public Card.CanBeGrabbedFunctionDelegate CanBeGrabbedFunction;
        public Card.CanBeStackedFunctionDelegate CanBeStackedFunction;

        public Card StackedCard { get; protected set; }
        public Action OnBecameEmpty;

        private void Awake()
        {
            CanBeGrabbedFunction = CardCanBeGrabbed;
            CanBeStackedFunction = CanBeStacked;
        }

        public abstract bool CardCanBeGrabbed(Card cardToGrab);
        public abstract bool CanBeStacked(Card parent, Card child);
        public abstract bool CanStack(Card card);

        public bool CanBeGrabbed() => false;
        public virtual Vector3 GetStackedWorldPosition() => transform.position;      

        public virtual void ForcedStack(Card card)
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

        public virtual bool Stack(Card card)
        {
            if (StackedCard == null)
            {
                card.Parent = this;

                BoundChild(card);
                StackedCard.transform.localPosition = new Vector3(0, 0f, -0.1f);

                return true;
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