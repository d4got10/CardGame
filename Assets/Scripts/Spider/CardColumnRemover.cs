using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CardGame.Spyder
{
    public class CardColumnRemover : MonoBehaviour
    {
        private void Awake() => Card.OnAnyCardStacked += CheckColumns;
        

        public void CheckColumns()
        {
            var cards = FindObjectsOfType<Card>().Where(t => !t.Hidden);
            foreach(var card in cards)
            {
                if(card.Value == Values.King)
                    if (CheckFullColumn(card))
                        Destroy(card.gameObject);
            }
        }

        public bool CheckFullColumn(Card card)
        {
            if (card.Value == Values.Ace)
                return true;
            if (card.StackedCard != null && card.StackedCard.Value == card.Value - 1 && card.StackedCard.Suit == card.Suit)
                return CheckFullColumn(card.StackedCard);

            return false;
        }
    }
}
