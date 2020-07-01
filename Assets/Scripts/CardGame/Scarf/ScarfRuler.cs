using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Scarf
{
    public class ScarfRuler : GameRuler
    {
        public void Start()
        {
            FindObjectOfType<Deck>()?.StartGame(4);
        }

        public override int[] GetPlaceHoldersCardCountSetup()
        {
            return new int[]{ 1, 2, 3, 4, 5, 6, 7};
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
