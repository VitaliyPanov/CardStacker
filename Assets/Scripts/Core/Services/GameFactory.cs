using CardStacker.Core.GameControllers;
using CardStacker.General.Controllers;
using CardStacker.General.Services;
using UnityEngine;

namespace CardStacker.Core.Services
{
    internal sealed class GameFactory : IGameFactory
    {
        private const string c_gameController = "[CORECONTROLLER]";
        private readonly IDataService _dataService;
        private readonly ILogicController _logicController;
        private readonly ICameraController _cameraController;
        private readonly IUIController _uiController;
        private readonly IControllersMediator _mediator;

        public GameFactory(IDataService dataService, ILogicController logicController,
            ICameraController cameraController, IUIController uiController, IControllersMediator mediator)
        {
            _dataService = dataService;
            _logicController = logicController;
            _cameraController = cameraController;
            _uiController = uiController;
            _mediator = mediator;
        }

        public void LoadData() => _dataService.Load();

        public void CreateGameController()
        {
            GameObject gameController = new GameObject(c_gameController);
            gameController.AddComponent<CoreController>()
                .Construct(_dataService, _logicController, _cameraController, _uiController, _mediator);
        }
    }
}