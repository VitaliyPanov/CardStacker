using System.Threading.Tasks;
using UnityEngine;

namespace CardStacker.Core
{
    public class LoadingScreen : MonoBehaviour
    {
        private CanvasGroup _screen;

        private void Awake()
        {
            _screen = GetComponent<CanvasGroup>();
            DontDestroyOnLoad(this);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _screen.alpha = 1f;
        }

        public void Hide() => FadeIn();

        private async void FadeIn()
        {
            float fadeStep = 0.03f;
            while (_screen.alpha > 0)
            {
                _screen.alpha -= fadeStep;
                await Task.Delay(30);
            }
            Destroy(gameObject);
        }
    }
}