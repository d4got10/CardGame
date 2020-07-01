using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class DeckTypePicker : MonoBehaviour
    {
        public void OnClick(int num)
        {
            FindObjectOfType<Deck>().StartGame(num);
        }
    }
}
