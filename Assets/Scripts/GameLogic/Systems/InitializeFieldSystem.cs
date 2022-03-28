using CardStacker.Data;
using CardStacker.GameLogic.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace CardStacker.GameLogic.Systems
{
    internal sealed class InitializeFieldSystem : IEcsInitSystem
    {
        private readonly StaticData _staticData;

        public InitializeFieldSystem(StaticData staticData) => _staticData = staticData;

        public void Init(EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            var cellPool = world.GetPool<CellComponent>();
            var emptyPool = world.GetPool<EmptyCellComponent>();
            var positionIdPool = world.GetPool<PositionIdComponent>();
            int[,] field = new int[_staticData.FieldWidth, _staticData.FieldHeight];
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    var entity = world.NewEntity();
                    field[i, j] = entity;
                    cellPool.Add(entity);
                    emptyPool.Add(entity);
                    positionIdPool.Add(entity).Value = new Vector2Int(i, j);
                }
            }

            InitializeHero(world, field);
        }

        private static void InitializeHero(EcsWorld world, int[,] field)
        {
            world.GetPool<HeroComponent>().Add(field[Mathf.CeilToInt(field.GetLength(0) * 0.5f - 1),
                Mathf.CeilToInt(field.GetLength(1) * 0.5f - 1)]);
        }
    }
}