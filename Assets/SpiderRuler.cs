using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Spyder
{
    public class SpiderRuler : GameRuler
    {
        [SerializeField] private SpyderWinListener _winListener;

        public override bool CanBeStacked(Card parent, Card child)
        {
            return (parent.StackedCard == null) && (parent.Value == child.Value + 1);
        }

        public override bool CanBeGrabbed(Card card)
        {
            if(card.StackedCard == null)
            {
                return true;
            }
            else
            {
                if (card.Value == card.StackedCard.Value + 1 && card.Suit == card.StackedCard.Suit)
                    return CanBeGrabbed(card);
                
                return false;
            }
        }

        public override void CheckWinCondition()
        {
            _winListener.CheckWin();
        }

        public override int[] GetPlaceHoldersCardCountSetup()
        {
            return new int[] { 5, 5, 5, 5, 5, 5, 4, 4, 4, 4 };
        }

        public override void OnDeckClick()
        {
            throw new System.NotImplementedException();
        }
    }
}
