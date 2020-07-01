using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public abstract class GameRuler : MonoBehaviour
    {
        public Card.BoolFunctionDelegate CanBeGrabbedFunction;
        public Card.CanBeStackedFunctionDelegate CanBeStackedFunction;

        private void Awake()
        {
            var rulers = FindObjectsOfType<GameRuler>();
            if (rulers != null && rulers.Length > 1)
            {
                Debug.LogError("There are more than 1 GameRuler on the scene!", this);
            }
        }

        public abstract bool CanBeStacked(Card parent, Card child);

        public abstract bool CanBeGrabbed(Card card);

        public abstract void OnDeckClick();

        public abstract void CheckWinCondition();

        public abstract int[] GetPlaceHoldersCardCountSetup();
    }
}
