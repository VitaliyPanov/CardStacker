using UnityEngine;

namespace CardStacker.Core
{
    internal sealed class GameStarter : MonoBehaviour
    {
        [SerializeField] private Bootstrapper _bootstrapPrefab;

        private void Awake()
        {
            Bootstrapper bootstrapper = FindObjectOfType<Bootstrapper>();
            if (bootstrapper == null)
            {
                Instantiate(_bootstrapPrefab);
            }

            Destroy(gameObject);
        }
    }
}