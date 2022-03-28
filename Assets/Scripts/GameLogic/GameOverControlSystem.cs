using CardStacker.Data;
using CardStacker.GameLogic.Components.Events;
using CardStacker.GameLogic.Services;
using CardStacker.General.Controllers;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace CardStacker.GameLogic
{
    internal sealed class GameOverControlSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<GameOverEvent>> _eventFilter = WorldNames.EVENTS;
        private readonly RuntimeData _runtimeData;
        private readonly IControllersMediator _controllersMediator;

        public GameOverControlSystem(RuntimeData runtimeData, IControllersMediator controllersMediator)
        {
            _runtimeData = runtimeData;
            _controllersMediator = controllersMediator;
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _eventFilter.Value)
            {
                _controllersMediator.GameOver();
            }
        }
    }
}