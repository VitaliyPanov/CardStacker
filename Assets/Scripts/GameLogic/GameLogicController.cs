using CardStacker.GameLogic.Components.Events;
using CardStacker.GameLogic.Services;
using CardStacker.GameLogic.Systems;
using CardStacker.General.Controllers;
using CardStacker.General.Services;
using CardStacker.General.Services.Input;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
#if UNITY_EDITOR
using Leopotam.EcsLite.UnityEditor;
#endif

namespace CardStacker.GameLogic
{
    public sealed class GameLogicController : ILogicController, IStart, IUpdate, IDestroy
    {
        private readonly IDataService _dataService;
        private readonly IViewService _viewService;
        private readonly IInputService _inputService;
        private EcsWorld _world;
        private EcsWorld _eventsWorld;
        private EcsSystems _initSystems;
        private EcsSystems _updateSystems;

        public GameLogicController(IDataService dataService, IViewService viewService, IInputService inputService)
        {
            _dataService = dataService;
            _viewService = viewService;
            _inputService = inputService;
        }

        public void Initialize(IControllersMediator mediator)
        {
            _world = new EcsWorld();
            _eventsWorld = new EcsWorld();

            _initSystems = new EcsSystems(_world);
            _initSystems
                .Add(new InitializeFieldSystem(_dataService.StaticData))
                .Inject();

            _updateSystems = new EcsSystems(_world);
            _updateSystems
                .AddWorld(_eventsWorld, WorldNames.EVENTS)
                .Add(new InputSystem(_inputService))
                .Add(new InstantiateCardsSystem(_viewService, _dataService.CardData, _dataService.RuntimeData))
                .Add(new InitializeCardsSystem(_dataService.CardData, _dataService.RuntimeData))
                .Add(new HeroCellMoveSystem(_dataService.StaticData))
                .Add(new HeroCardMoveSystem())
                .Add(new HeroPointsControlSystem(_dataService.RuntimeData, mediator))
                .Add(new CardRemoveSystem(_viewService))
                .Add(new GameOverControlSystem(_dataService.RuntimeData, mediator))

#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem())
                .Add(new EcsWorldDebugSystem(WorldNames.EVENTS))
#endif
                .AddOneFrameSystem<NewCardEvent>(WorldNames.EVENTS)
                .AddOneFrameSystem<TryMoveEvent>(WorldNames.EVENTS)
                .AddOneFrameSystem<ApplyMoveOnCellEvent>(WorldNames.EVENTS)
                .AddOneFrameSystem<HeroPointsChangedEvent>(WorldNames.EVENTS)
                .AddOneFrameSystem<RemoveCardEvent>(WorldNames.EVENTS)
                .AddOneFrameSystem<GameOverEvent>(WorldNames.EVENTS)
                .Inject();
        }

        public void Start()
        {
            _initSystems.Init();
            _updateSystems.Init();

            _initSystems.Destroy();
            _initSystems = null;
        }

        public void Update() => _updateSystems.Run();

        public void Destroy()
        {
            if (_initSystems != null)
            {
                _initSystems.Destroy();
                _initSystems = null;
            }

            if (_updateSystems != null)
            {
                _updateSystems.Destroy();
                _updateSystems = null;
            }

            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }

            if (_eventsWorld != null)
            {
                _eventsWorld.Destroy();
                _eventsWorld = null;
            }
        }
    }
}