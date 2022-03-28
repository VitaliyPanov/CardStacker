using System.Linq;
using CardStacker.GameLogic.Components;
using CardStacker.GameLogic.Components.Events;
using CardStacker.GameLogic.Services;
using CardStacker.GameLogic.Views;
using DG.Tweening;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace CardStacker.GameLogic
{
    public sealed class HeroCardMoveSystem : IEcsRunSystem
    {
        private const float c_moveDuration = 1f;
        private readonly EcsFilterInject<Inc<ApplyMoveOnCellEvent>> _eventFilter = WorldNames.EVENTS;
        private readonly EcsPoolInject<ApplyMoveOnCellEvent> _eventPool = WorldNames.EVENTS;
        private readonly EcsFilterInject<Inc<HeroComponent, CellComponent>> _heroCellFilter = default;
        private readonly EcsPoolInject<EmptyCellComponent> _emptyPool = default;
        private readonly EcsPoolInject<CardEntityComponent> _cardPool = default;
        private readonly EcsPoolInject<CardPointsComponent> _pointsPool = default;
        private readonly EcsPoolInject<HeroComponent> _heroPool = default;
        private readonly EcsPoolInject<TransformComponent> _transformPool = default;
        private readonly EcsPoolInject<CardTypeComponent> _typePool = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var i in _eventFilter.Value)
            {
                ref var targetCellEntity = ref _eventPool.Value.Get(i).Entity;
                ref var targetCardEntity = ref _cardPool.Value.Get(targetCellEntity).Value;
                ref var targetPoints = ref _pointsPool.Value.Get(targetCardEntity);
                ref var targetType = ref _typePool.Value.Get(targetCardEntity).Value;
                
                systems.SetEvent<HeroPointsChangedEvent>().Value =
                    targetType == CardType.Green ? targetPoints.Value : - targetPoints.Value;
                systems.SetEvent<RemoveCardEvent>().Entity = targetCardEntity;
                
                var heroCellEntity = _heroCellFilter.Value.GetRawEntities().First();
                ref var heroCardEntity = ref _cardPool.Value.Get(heroCellEntity);
                ref var heroTransform = ref _transformPool.Value.Get(heroCardEntity.Value).Value;
                ref var targetTransform = ref _transformPool.Value.Get(targetCardEntity).Value;
                heroTransform.DOMove(targetTransform.position, c_moveDuration);
                
                UpdateCell(heroCellEntity, targetCellEntity, heroCardEntity);
            }
        }

        private void UpdateCell(int heroCellEntity, int targetCellEntity, CardEntityComponent heroCardEntity)
        {
            EcsPool<HeroComponent> heroPool = _heroPool.Value;
            EcsPool<CardEntityComponent> cardPool = _cardPool.Value;
            heroPool.Del(heroCellEntity);
            heroPool.Add(targetCellEntity);
            cardPool.Del(heroCellEntity);
            cardPool.Del(targetCellEntity);
            cardPool.Add(targetCellEntity).Value = heroCardEntity.Value;
            _emptyPool.Value.Add(heroCellEntity);
        }
    }
}