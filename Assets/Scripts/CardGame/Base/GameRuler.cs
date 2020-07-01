using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public abstract class GameRuler : MonoBehaviour
    {
        public void Awake()
        {
            var rulers = FindObjectsOfType<GameRuler>();
            if (rulers != null && rulers.Length > 1)
            {
                Debug.LogError("There are more than 1 GameRuler on the scene!", this);
            }
        }

        public abstract int[] GetPlaceHoldersCardCountSetup();
    }
}
