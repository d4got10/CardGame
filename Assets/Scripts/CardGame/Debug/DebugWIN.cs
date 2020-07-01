using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardGame.Scarf;
using CardGame;

public class DebugWIN : MonoBehaviour
{
    [SerializeField] private House[] _houses;
    [SerializeField] private Deck _deck;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            for (int suit = 1; suit <= 4; suit++) {
                for (int value = 1; value <= 13; value++) {
                    if (suit == 4 && value == 13) continue;
                    _houses[suit-1].ForcedStack(_deck.CreateCard(Vector3.zero, (Suits)suit, (Values)value, false));
                }
            }
            Card.OnAnyCardStacked?.Invoke();
        }
    }
}
