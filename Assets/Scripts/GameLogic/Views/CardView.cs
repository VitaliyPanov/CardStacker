using TMPro;
using UnityEngine;

namespace CardStacker.GameLogic.Views
{
    public class CardView : MonoBehaviour, ICardView
    {
        [SerializeField] private TextMeshProUGUI _pointsField;
        [SerializeField] private SpriteRenderer _cardRenderer;
        [SerializeField] private SpriteRenderer _overlayRenderer;
        private int _pointsValue;
        private Canvas _canvas;
        public Transform Transform => transform;
        public GameObject GameObject => gameObject;

        private ValueSlider _valueSlider;

        private void Awake() => _canvas = transform.GetComponentInChildren<Canvas>();

        public void InitializeView() => _pointsValue = 0;

        public void SetOverlay(Sprite sprite, int layerOrder)
        {
            _overlayRenderer.sprite = sprite;
            _cardRenderer.sortingOrder = layerOrder;
            _overlayRenderer.sortingOrder = layerOrder + 1;
            _canvas.sortingOrder = layerOrder + 2;
        }

        public void SetPoints(int newValue)
        {
            int previousValue = _pointsValue;
            _pointsValue = newValue;
            AddPoints(previousValue, _pointsValue);
        }

        private void AddPoints(int from, int to)
        {
            if (from == to) return;
            if (_valueSlider != null)
            {
                _valueSlider.Stop();
                _valueSlider = null;
                _pointsField.text = $"{from}";
            }

            _valueSlider = new ValueSlider(_pointsField);
            _valueSlider.Start(from, to);
        }
    }
}