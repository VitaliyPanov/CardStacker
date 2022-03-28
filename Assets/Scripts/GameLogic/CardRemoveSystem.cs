using CardStacker.GameLogic.Components;
using CardStacker.GameLogic.Components.Events;
using CardStacker.GameLogic.Services;
using CardStacker.General.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace CardStacker.GameLogic
{
    internal sealed class CardRemoveSystem : IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsFilterInject<Inc<RemoveCardEvent>> _eventFilter = WorldNames.EVENTS;
        private readonly EcsPoolInject<RemoveCardEvent> _eventPool = WorldNames.EVENTS;
        private readonly EcsPoolInject<CardViewComponent> _viewPool = default;
        
        private readonly IViewService _viewService;

        public CardRemoveSystem(IViewService viewService) => _viewService = viewService;

        public void Run(EcsSystems systems)
        {
            foreach (var i in _eventFilter.Value)
            {
                ref var cardEntity = ref _eventPool.Value.Get(i).Entity;
                ref var cardView = ref _viewPool.Value.Get(cardEntity).Value;
                _viewService.DestroyView(cardView);
                _world.Value.DelEntity(cardEntity);
            }
        }
    }
}