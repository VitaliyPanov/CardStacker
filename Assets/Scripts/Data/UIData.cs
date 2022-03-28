using UnityEngine;
namespace CardStacker.Data
{
    [CreateAssetMenu(fileName = "UIData", menuName = "CardStacker/UIData")]
    public sealed class UIData : ScriptableObject
    {
        public GameObject GameHUD;
    }
}