using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace CardGame
{
    [CreateAssetMenu(fileName = "CardAnimationFactory", menuName = "Cards/Card Animation Factory ScriptableObject", order = 1)]
    public class CardAnimationFactory : ScriptableObject
    {
        [SerializeField] private VideoClip[] Spades;
        [SerializeField] private VideoClip[] Diamonds;
        [SerializeField] private VideoClip[] Hearts;
        [SerializeField] private VideoClip[] Clubs;

        public VideoClip GetAnimationVideo(Suits Suit, Values Value)
        {
            switch ((int)Suit - 1)
            {
                case 0:
                    return Spades[(int)Value - 1];
                case 1:
                    return Diamonds[(int)Value - 1];
                case 2:
                    return Hearts[(int)Value - 1];
                case 3:
                    return Clubs[(int)Value - 1];
                default:
                    return null;
            }
        }
    }
}