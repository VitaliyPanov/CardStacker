using UnityEngine;

namespace CardStacker.Data
{
    [CreateAssetMenu(fileName = "StaticData", menuName = "CardStacker/StaticData")]
    public sealed class StaticData : ScriptableObject
    {
        [Range(1, 30)] public int FieldWidth;
        [Range(1, 30)] public int FieldHeight;
        [Range(0, 1f)] public float MaxRedChanceRate = 0.5f;
        public int DifficultyStepMoves = 10;
    }
}