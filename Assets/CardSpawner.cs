using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame 
{
    public class CardSpawner : MonoBehaviour
    {
        [SerializeField] private CardFactoryObject _cardFactory;
        private void Awake()
        {
            _cardFactory.CreateCard(Suits.Clubs, Values.Five, false).transform.position = new Vector3(0,0,-0.1f);
            _cardFactory.CreateCard(Suits.Diamonds, Values.Four, false).transform.position = new Vector3(1, 0, -0.1f);
            _cardFactory.CreateCard(Suits.Diamonds, Values.Five, false).transform.position = new Vector3(-1, 0, -0.1f);
        }
    }
}
