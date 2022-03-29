using UnityEngine;

namespace CardStacker.Data
{
    [CreateAssetMenu(fileName = "StaticData", menuName = "CardStacker/StaticData")]
    public sealed class StaticData : ScriptableObject
    {
        [Range(3, 30)] public int FieldColumns;
        [Range(3, 30)] public int FieldRows;
        [Range(0, 1f)] public float MaxRedChanceRate = 0.5f;
        public const int DifficultyStepMoves = 10;
    }
}