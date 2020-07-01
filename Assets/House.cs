using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Scarf
{
    public class House : PlaceHolder
    {
        private List<Card> _stackedCards;

        public override bool CanStack(Card card)
        {
            if (StackedCard == null)
                return (card.Value == Values.Ace);
            else
                return ((card.Suit == StackedCard.Suit) && (card.Value == StackedCard.Value + 1));
        }

        private void OnMouseDown()
        {
            StackedCard.OnMouseDown();
        }

        protected override void BoundChild(Card card)
        {
            if (_stackedCards == null)
                _stackedCards = new List<Card>();

            StackedCard = card;
            StackedCard.transform.parent = transform;
            StackedCard.OnChangedParent += UnStack;
            StackedCard.IsWinnable = (StackedCard.Value == Values.King);
            StackedCard.GetComponent<BoxCollider2D>().enabled = false;
        }

        public override void UnStack(IStackable newParent)
        {
            if((object)newParent != this)
            {
                StackedCard.OnChangedParent -= UnStack;
                _stackedCards.RemoveAt(_stackedCards.Count - 1);
                StackedCard = _stackedCards[_stackedCards.Count - 1];
            }
        }
    }
}
