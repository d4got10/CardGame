using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Spyder
{
    public class ColumnHolder : PlaceHolder, IWinListenable
    {
        public void AddListener(IWinListener listener) => OnBecameEmpty += listener.Listen;
        public bool GetWinState() => StackedCard == null;

        public override bool CanBeStacked(Card parent, Card child) => (parent.Value == child.Value + 1);

        public override bool CanStack(Card card) => true;

        public override bool CardCanBeGrabbed(Card cardToGrab)
        {
            if (cardToGrab.StackedCard == null)
                return true;
            else if(cardToGrab.Suit == cardToGrab.StackedCard.Suit && 
                    cardToGrab.Value == cardToGrab.StackedCard.Value + 1)
                return CardCanBeGrabbed(cardToGrab.StackedCard);
            return false;
        }
    }
}
