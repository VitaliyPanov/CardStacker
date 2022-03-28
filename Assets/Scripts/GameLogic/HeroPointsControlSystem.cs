using System.Linq;
using CardStacker.Data;
using CardStacker.GameLogic.Components;
using CardStacker.GameLogic.Components.Events;
using CardStacker.GameLogic.Services;
using CardStacker.General.Controllers;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace CardStacker.GameLogic
{
    internal sealed class HeroPointsControlSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<HeroPointsChangedEvent>> _eventFilter = WorldNames.EVENTS;
        private readonly EcsFilterInject<Inc<HeroComponent, CardPointsComponent, CardViewComponent>> _heroFilter =
            default;
        private readonly EcsPoolInject<HeroPointsChangedEvent> _eventPool = WorldNames.EVENTS;
        private readonly EcsPoolInject<CardPointsComponent> _pointsPool = default;
        private readonly EcsPoolInject<CardViewComponent> _viewPool = default;
        private readonly IControllersMediator _controllersMediator;
        private readonly RuntimeData _runtimeData;
        private int _movesValue;

        public HeroPointsControlSystem(RuntimeData runtimeData, IControllersMediator controllersMediator)
        {
            _runtimeData = runtimeData;
            _controllersMediator = controllersMediator;
        }

        public void Init(EcsSystems systems) => _movesValue = 0;

        public void Run(EcsSystems systems)
        {
            foreach (var i in _eventFilter.Value)
            {
                ref var points = ref _eventPool.Value.Get(i).Value;
                var heroEntity = _heroFilter.Value.GetRawEntities().First();
                ref var heroPoints = ref _pointsPool.Value.Get(heroEntity);
                ref var heroView = ref _viewPool.Value.Get(heroEntity).Value;
                var newValue = points + heroPoints.Value;
                heroView.SetPoints(newValue);
                heroPoints.Value = newValue;
                _controllersMediator.IncreaseScore();
                UpdateMovesValue();
                if (heroPoints.Value <= 0)
                    systems.SetEvent<GameOverEvent>();
            }
        }

        private void UpdateMovesValue()
        {
            _movesValue++;
            while (_movesValue >= _runtimeData.DifficultyStepValue)
            {
                _movesValue -= _runtimeData.DifficultyStepValue;
                _controllersMediator.IncreaseLevel();
            }
        }
    }
}