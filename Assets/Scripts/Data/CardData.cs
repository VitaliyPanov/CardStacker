using UnityEngine;

namespace CardStacker.Data
{
    [CreateAssetMenu(fileName = "CardData", menuName = "CardStacker/CardData")]
    public sealed class CardData : ScriptableObject
    {
        public GameObject CardPrefab;
        public Sprite HeroOverlay;
        public Sprite GreenOverlay;
        public Sprite RedOverlay;
    }
}
