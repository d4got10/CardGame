using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(fileName = "CardFactory", menuName = "Cards/Card Factory ScriptableObject", order = 1)]
    public class CardFactoryObject : ScriptableObject
    {
        [SerializeField] private GameObject _cardPrefab;

        [SerializeField] private Sprite[] _cardFaces;

        public GameObject CreateCard(Suits suit, Values value, bool hidden)
        {
            GameObject card = Instantiate(_cardPrefab);

            card.GetComponent<SpriteRenderer>().sprite = _cardFaces[((int)suit - 1) * 13 + ((int)value - 1)];
            card.GetComponent<Card>().Set(suit, value, hidden);

            return card;
        }
    }
}