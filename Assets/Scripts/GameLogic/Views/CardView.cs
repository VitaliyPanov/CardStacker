using System.Threading.Tasks;
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

        private void Awake()
        {
            _canvas = transform.GetComponentInChildren<Canvas>();
        }

        public void InitializeView() => _pointsValue = 0;

        public void SetPoints(int value)
        {
            int currentPoints = _pointsValue;
            _pointsValue = value;
            AddPointsAsync(currentPoints, (value - currentPoints) > 0);
        }

        public void SetOverlay(Sprite sprite, int layerOrder)
        {
            _overlayRenderer.sprite = sprite;
            _cardRenderer.sortingOrder = layerOrder;
            _overlayRenderer.sortingOrder = layerOrder + 1;
            _canvas.sortingOrder = layerOrder + 2;
        }

        private async void AddPointsAsync(int currentPoints, bool isAdd)
        {
            if (isAdd)
            {
                while (_pointsValue >= currentPoints)
                {
                    _pointsField.text = $"{currentPoints++}";
                    await Task.Delay(100);
                }
            }
            else
            {
                while (_pointsValue <= currentPoints)
                {
                    _pointsField.text = $"{currentPoints--}";
                    await Task.Delay(100);
                }
            }
        }
    }
}