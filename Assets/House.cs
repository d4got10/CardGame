using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Scarf
{
    public class House : PlaceHolder, IWinListenable
    {
        public Action OnWin;

        private void Awake() => StackedCardYOffset = 0;

        public void AddListener(IWinListener listener) => OnWin += listener.Listen;
        public bool GetWinState() => StackedCard.Value == Values.King;

        public override bool CanStack(Card card)
        {
            if (StackedCard == null)
                return (card.Value == Values.Ace);
            else
                return ((card.Suit == StackedCard.Suit) && (card.Value == StackedCard.Value + 1));
        }

        public override bool CanBeStacked(Card parent, Card child)
        {
            if(parent.StackedCard == null)
            {
                if (child.Suit == parent.Suit && child.Value + 1 == parent.Value)
                    return true;
            }
            return false;
        }

        public override bool CardCanBeGrabbed(Card cardToGrab) => cardToGrab.StackedCard == null;        
    }
}
