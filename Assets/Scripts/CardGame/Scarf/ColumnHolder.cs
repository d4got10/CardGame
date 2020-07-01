using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Scarf
{
    public class ColumnHolder : PlaceHolder
    {
        public override bool CanStack(Card card) => card.Value == Values.King;

        public override bool CardCanBeGrabbed(Card cardToGrab)
        {
            if (cardToGrab.StackedCard == null)
                return true;
            else
            {
                var differentColorSuits = ScarfRuler.GetDifferentColorSuit(cardToGrab.StackedCard.Suit);
                if ((cardToGrab.Suit == differentColorSuits[0] || cardToGrab.Suit == differentColorSuits[1]) &&
                     (cardToGrab.Value == cardToGrab.StackedCard.Value + 1))
                    return cardToGrab.StackedCard.CanBeGrabbed();
                
            }
            return false; 
        }
        public override bool CanBeStacked(Card parent, Card child)
        {
            var differentColorSuits = ScarfRuler.GetDifferentColorSuit(child.Suit);
            if (parent.Suit == differentColorSuits[0] || parent.Suit == differentColorSuits[1])
            {
                if (parent.Value == child.Value + 1)
                    return true;
            }
            return false;
        }
    }
}
