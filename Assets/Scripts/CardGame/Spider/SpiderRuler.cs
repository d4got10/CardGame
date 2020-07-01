using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Spyder
{
    public class SpiderRuler : GameRuler
    {
        public override int[] GetPlaceHoldersCardCountSetup()
        {
            return new int[] { 5, 5, 5, 5, 5, 5, 4, 4, 4, 4 };
        }
    }
}
