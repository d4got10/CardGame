using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace CardGame
{
    public class WinListener : MonoBehaviour, IWinListener
    {
        public UnityEvent OnWin;

        private List<IWinListenable> _winListenables;

        private void Awake()
        {
            _winListenables = FindObjectsOfType<MonoBehaviour>().OfType<IWinListenable>().ToList();
            foreach (var listenable in _winListenables)
                listenable.AddListener(this);
        }

        public void Listen() => CheckWin();

        public void CheckWin()
        {
            bool allHoldersAreEmpty = true;
            foreach (var winListenable in _winListenables)
                if (!winListenable.GetWinState())
                    allHoldersAreEmpty = false;

            if (allHoldersAreEmpty) OnWin?.Invoke();
        }
    }
}