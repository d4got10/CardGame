using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class Pocket : PlaceHolder
    {
        public override bool CanBeStacked(Card parent, Card child) => false;

        public override bool CanStack(Card card) => (StackedCard == null) && (card.StackedCard == null);

        public override bool CardCanBeGrabbed(Card cardToGrab) => true;
    }
}
