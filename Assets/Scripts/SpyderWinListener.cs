using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CardGame.Spyder
{
    public class SpyderWinListener : MonoBehaviour
    {
        public UnityEvent OnWin;

        [SerializeField] private PlaceHolder[] _holders;

        private void Awake()
        {
            foreach (var holder in _holders)
            {
                holder.OnBecameEmpty += CheckWin;
            }
        }

        public void CheckWin()
        {
            bool allHoldersAreEmpty = true;
            foreach (var holder in _holders)
                if (holder.StackedCard != null) allHoldersAreEmpty = false;

            if (allHoldersAreEmpty) OnWin?.Invoke();
        }
    }
}