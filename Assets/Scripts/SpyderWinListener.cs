using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace CardGame.Spyder
{
    public class SpyderWinListener : MonoBehaviour, IWinListener
    {
        public UnityEvent OnWin;

        [SerializeField] private PlaceHolder[] _holders;

        private void Awake()
        {
            var listenables = FindObjectsOfType<MonoBehaviour>().OfType<IWinListenable>().ToList();
            foreach (var listenable in listenables)
                listenable.AddListener(this);
        }

        public void Listen() => CheckWin();

        public void CheckWin()
        {
            bool allHoldersAreEmpty = true;
            foreach (var holder in _holders)
                if (holder.StackedCard != null) allHoldersAreEmpty = false;

            if (allHoldersAreEmpty) OnWin?.Invoke();
        }
    }
}