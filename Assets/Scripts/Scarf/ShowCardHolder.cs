using UnityEngine;
namespace CardGame.Scarf
{
    public class ShowCardHolder : PlaceHolder
    {
        public override bool CanBeStacked(Card parent, Card child) => false;

        public override bool CanStack(Card card) => false;

        public override bool CardCanBeGrabbed(Card cardToGrab) => cardToGrab.StackedCard == false;
    }
}