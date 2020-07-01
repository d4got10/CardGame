using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class NextMoveHelper : MonoBehaviour
    {
        private Dictionary<Values, List<Card>> _checkedCards;
        private List<(IStackable, Card)> _moves = new List<(IStackable, Card)>();

        [SerializeField] private Deck _deck;
        [SerializeField] private PlaceHolder[] _holders;


        private bool _isClicked = false;
        private int _moveNumber = 0;

        private void OnEnable() => Card.OnAnyCardChangedPlace += ClearMoves;
        private void OnDisable() => Card.OnAnyCardChangedPlace -= ClearMoves;

        private void ClearMoves() => _moves = new List<(IStackable, Card)>();

        public void FindNextMove()
        {
            if (_isClicked || !_deck.HasStartedGame) return;

            StartCoroutine(IsClickedCoroutine(1.3f));

            if (_moves.Count == 0)
            {
                _moveNumber = 0;

                _checkedCards = new Dictionary<Values, List<Card>>();

                Card[] cards = FindObjectsOfType<Card>();

                ScanCards(cards);
                CalculateMoves(cards);
            }

            if (_moves.Count == 0)
                _deck.OnClick();
            else
            {
                var cardPair = _moves[_moveNumber % _moves.Count];

                cardPair.Item2.MoveToGhostly(cardPair.Item1.GetStackedWorldPosition());
                _moveNumber++;
            }
        }

        private void ScanCards(Card[] cards)
        {
            foreach (var card in cards)
            {
                if (card.Hidden || card.StackedCard != null) continue;

                if (!_checkedCards.ContainsKey(card.Value))
                {
                    _checkedCards.Add(card.Value, new List<Card> { card });
                }
                else
                {
                    _checkedCards[card.Value].Add(card);
                }
            }
        }

        private void CalculateMoves(Card[] cards)
        {
            foreach (var card in cards)
            {
                if (card.Hidden || !card.CanBeGrabbed()) continue;

                if (card.Value == Values.King)
                {
                    foreach (var holder in _holders)
                    {
                        if (holder.StackedCard == null && holder.CanStack(card))
                            _moves.Add((holder, card));
                    }
                }
                else 
                {
                    foreach (var holder in _holders)
                    {
                        if (holder.StackedCard == null && holder.CanStack(card))
                        {
                            _moves.Add((holder, card));
                        }
                    }
                    if (_checkedCards.ContainsKey(card.Value + 1))
                        foreach (var parent in _checkedCards[card.Value + 1])
                        {
                            if(parent.CanBeStackedFunction(parent, card))
                                _moves.Add((parent, card));
                        }
                }
            }
        }

        IEnumerator IsClickedCoroutine(float time)
        {
            _isClicked = true;
            yield return new WaitForSeconds(time);
            _isClicked = false;
        }
    }
}
