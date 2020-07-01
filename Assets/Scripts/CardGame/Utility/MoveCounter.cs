using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class MoveCounter : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private Deck _deck;

        public int MoveCount { get; private set; } = 0;

        private void Awake()
        {
            MoveCount = 0;
        }

        private void OnEnable() => Card.OnAnyCardStacked += AddMove;

        private void OnDisable() => Card.OnAnyCardStacked -= AddMove;


        private void AddMove()
        {
            MoveCount++;
            _text.text = $"Moves: {MoveCount}";
        }
    }
}
