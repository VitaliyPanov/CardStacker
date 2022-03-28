using System;
using System.Linq;
using CardStacker.Data;
using CardStacker.GameLogic.Components;
using CardStacker.GameLogic.Components.Events;
using CardStacker.GameLogic.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CardStacker.GameLogic
{
    internal sealed class HeroCellMoveSystem : IEcsInitSystem,IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<TryMoveEvent>> _eventFilter = WorldNames.EVENTS;
        private readonly EcsFilterInject<Inc<HeroComponent, CellComponent, PositionIdComponent>> _heroCellFilter = default;
        private readonly EcsFilterInject<Inc<CellComponent, PositionIdComponent>, Exc<HeroComponent>> _cellFilter = default;
        private readonly EcsPoolInject<TryMoveEvent> _eventPool = WorldNames.EVENTS;
        private readonly EcsPoolInject<PositionIdComponent> _positionIdPool = default;
        private readonly StaticData _staticData;
        private EcsSystems _systems;

        public HeroCellMoveSystem(StaticData staticData) => _staticData = staticData;

        public void Init(EcsSystems systems) => _systems = systems;

        public void Run(EcsSystems systems)
        {
            foreach (var i in _eventFilter.Value)
            {
                var heroCellId = _heroCellFilter.Value.GetRawEntities().First();
                Vector2Int heroCellPosition = _positionIdPool.Value.Get(heroCellId).Value;
                switch (_eventPool.Value.Get(i).Direction)
                {
                    case MoveDirection.Right:
                    {
                        if (heroCellPosition.x == _staticData.FieldWidth - 1) return;
                        ChangeHeroCell(new Vector2Int(heroCellPosition.x + 1, heroCellPosition.y), heroCellId);
                        break;
                    }
                    case MoveDirection.Left:
                    {
                        if (heroCellPosition.x == 0) return;
                        ChangeHeroCell(new Vector2Int(heroCellPosition.x - 1, heroCellPosition.y), heroCellId);
                        break;
                    }
                    case MoveDirection.Up:
                    {
                        if (heroCellPosition.y == _staticData.FieldHeight - 1) return;
                        ChangeHeroCell(new Vector2Int(heroCellPosition.x, heroCellPosition.y + 1), heroCellId);
                        break;
                    }
                    case MoveDirection.Down:
                    {
                        if (heroCellPosition.y == 0) return;
                        ChangeHeroCell(new Vector2Int(heroCellPosition.x, heroCellPosition.y - 1), heroCellId);
                        break;
                    }
                    case MoveDirection.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void ChangeHeroCell(Vector2Int targetPosition, int heroPosition)
        {
            foreach (var j in _cellFilter.Value)
            {
                ref var cellPosition = ref _positionIdPool.Value.Get(j).Value;
                if (cellPosition == targetPosition)
                {
                    _systems.SetEvent<ApplyMoveOnCellEvent>().Entity = j;
                    break;
                }
            }
        }
    }
}