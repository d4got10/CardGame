using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Scarf
{
    public class ScarfRuler : GameRuler
    {
        [SerializeField] private House[] _houses;

        private void Awake()
        {
            FindObjectOfType<Deck>()?.StartGame(4);
        }

        public override bool CanBeStacked(Card parent, Card child)
        {
            if(parent.StackedCard == null)
            {
                var differentColorSuits = GetDifferentColorSuit(child.Suit);
                if(parent.Value == child.Value + 1 && (parent.Suit == differentColorSuits[0] || parent.Suit == differentColorSuits[1]))
                {
                    return true;
                }
            }
            return false;
        }

        public override bool CanBeGrabbed(Card card)
        {
            if (card.StackedCard == null)
            {
                return true;
            }
            else
            {
                var differentColorSuits = GetDifferentColorSuit(card.Suit);
                if (card.Value == card.StackedCard.Value + 1 &&
                    (card.StackedCard.Suit == differentColorSuits[0] || card.StackedCard.Suit == differentColorSuits[1]))
                    return CanBeGrabbed(card.StackedCard);
                
                return false;
            }
        }

        public override int[] GetPlaceHoldersCardCountSetup()
        {
            return new int[]{ 1, 2, 3, 4, 5, 6, 7};
        }

        public override void CheckWinCondition()
        {
            throw new System.NotImplementedException();
        }

        public override void OnDeckClick()
        {
            throw new System.NotImplementedException();
        }

        public static Suits[] GetDifferentColorSuit(Suits suit)
        {
            var suits = new Suits[2];
            if(Mathf.Abs((int)suit - 2.5f) < 1)
            {
                suits[0] = Suits.Clubs;
                suits[1] = Suits.Spades;
            }
            else
            {
                suits[0] = Suits.Diamonds;
                suits[1] = Suits.Hearts;
            }
            return suits;
        }
    }
}
