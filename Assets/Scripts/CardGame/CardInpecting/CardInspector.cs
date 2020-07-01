using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace CardGame
{
    public class CardInspector : MonoBehaviour
    {
        public InspectableCard CurrentInspectableCard { get; private set; }

        [SerializeField] private CardAnimationFactory _cardAnimationFactory;
        [Space]
        [SerializeField] private BoxCollider2D _clickOffArea;
        [SerializeField] private VideoPlayer _videoPlayer;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private GameObject _fade;

        public bool _isInspecting;

        private void Awake()
        {
            _clickOffArea.enabled = false;
            _meshRenderer.enabled = false;
            _fade.SetActive(false);
        }

        public void InspectObject(InspectableCard instance)
        {
            if (CurrentInspectableCard != null) return;

            CurrentInspectableCard = instance;
            _isInspecting = true;
            StartCoroutine(InspectingCoroutine());
        }

        private void OnMouseUp()
        {
            _clickOffArea.enabled = false;
            _meshRenderer.enabled = false;
            _fade.SetActive(false);
            _isInspecting = false;
        }

        IEnumerator InspectingCoroutine()
        {
            var clip = _cardAnimationFactory.GetAnimationVideo(CurrentInspectableCard.Card.Suit, CurrentInspectableCard.Card.Value);

            if (clip != null)
            {
                _clickOffArea.enabled = true;
                _videoPlayer.clip = clip;
                _videoPlayer.Play();
                _meshRenderer.enabled = true;
                _fade.SetActive(true);
            }
            else
            {
                _isInspecting = false;
            }

            while (_isInspecting)
                yield return null;

            _videoPlayer.Stop();

            CurrentInspectableCard = null;
        }
    }
}
