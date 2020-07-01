using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class InspectableCard : MonoBehaviour
    {
        public static Action<InspectableCard> OnAnyIspectableObjectPickedOnce;

        public Card Card;

        public bool CanBeInspected { get { return Card.StackedCard == null; } }

        private bool _isClicked;

        private void Awake() => OnAnyIspectableObjectPickedOnce += (obj) => { if (obj != this) _isClicked = false; };

        public void OnMouseUp()
        {
            if (!CanBeInspected) return;

            if (_isClicked)
            {
                StartInspecting();
                _isClicked = false;
            }
            else
            {
                OnAnyIspectableObjectPickedOnce?.Invoke(this);
                StartCoroutine(DelayedSetToFalse(0.3f));
                _isClicked = true;
            }
        }

        public void StartInspecting()
        {
            var inspector = FindObjectOfType<CardInspector>();

            inspector?.InspectObject(this);
        }

        IEnumerator DelayedSetToFalse(float time)
        {
            yield return new WaitForSeconds(time);
            _isClicked = false;
        }
    }
}
