using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Solitaire
{
    public class SolitaireRuler : GameRuler
    {
        public void Start()
        {
            FindObjectOfType<Deck>()?.StartGame(4, true);
        }

        public override int[] GetPlaceHoldersCardCountSetup()
        {
            return new int[] { 7, 7, 7, 7, 6, 6, 6, 6 };
        }
    }
}