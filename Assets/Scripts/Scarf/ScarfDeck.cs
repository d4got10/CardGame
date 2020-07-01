using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Scarf
{
    public class ScarfDeck : Deck
    {
        [SerializeField] private ShowCardHolder _showCardHolder;
        [SerializeField] private ShowCardHolder _hideCardHolder;

        private bool _canBeClicked = true;

        private void Awake() => _canBeClicked = true;

        public override void OnClick()
        {
            if(_canBeClicked)
                ShowCards();
        }

        public void ShowCards()
        {
            Card[] cards = new Card[3];
            if (_showCardHolder.StackedCard != null)
            {
                cards[2] = _showCardHolder.StackedCard;
                if(cards[2].StackedCard != null)
                {
                    cards[1] = cards[2].StackedCard;
                    cards[0] = cards[1].StackedCard;
                }
            }

            for(int i = 0; i < 3; i++)
            {
                if (cards[2 - i] != null)
                {
                    Cards.Add(cards[2 - i]);
                }
                if (cards[i] != null)
                {                 
                    cards[i].transform.parent = transform;
                    cards[i].OnChangedParent?.Invoke(null);
                    cards[i].Hidden = true;
                    cards[i].MoveTo(new Vector3(0, 0, 1));   
                }
            }

            for(int i = 0; i < 3; i++)
            {
                if (Cards.Count > 1)
                {
                    var card = Cards[0];
                    Cards.RemoveAt(0);
                    card.Open();
                    _showCardHolder.ForcedStack(card);
                }
            }
            if (Cards.Count == 0)
                Destroy(gameObject);

            StartCoroutine(TimeOutCoroutine(1f));
        }

        IEnumerator TimeOutCoroutine(float time)
        {
            _canBeClicked = false;
            yield return new WaitForSeconds(time);
            _canBeClicked = true;
        }
    }
}
