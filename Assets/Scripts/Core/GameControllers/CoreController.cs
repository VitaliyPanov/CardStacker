using System;
using CardStacker.General.Controllers;
using CardStacker.General.Services;
using UnityEngine;

namespace CardStacker.Core.GameControllers
{
    internal sealed class CoreController : MonoBehaviour, ICoreController
    {
        private const string _logicName = "logic";
        private const string _cameraName = "camera";
        public event Action OnControllerDestroyEvent;
        private ILogicController _logicController;
        private ICameraController _cameraController;
        private IUIController _uiController;
        private IDataService _dataService;
        private IControllersMediator _controllersMediator;
        private Controllers _controllers;

        public void Construct(IDataService dataService, ILogicController logicController,
            ICameraController cameraController, IUIController uiController, IControllersMediator controllersMediator)
        {
            _dataService = dataService;
            _logicController = logicController;
            _cameraController = cameraController;
            _uiController = uiController;
            _controllersMediator = controllersMediator;
            InitializeControllers();
            AddControllersToList();
        }
        
        private void InitializeControllers()
        {
            _controllersMediator.Initialize(this ,_uiController, _dataService, _dataService.RuntimeData);
            _logicController.Initialize(_controllersMediator);
            _cameraController.SetCamera(Camera.main);
            _uiController.Initialize(_dataService.UIData);
        }

        private void AddControllersToList()
        {
            _controllers = new Controllers();
            _controllers
                .Add(_cameraController, _cameraName)
                .Add(_logicController, _logicName);
        }

        public void DestroyCore()
        {
            OnControllerDestroyEvent?.Invoke();
            Destroy(gameObject);
        }

        public void DestroyLogic()
        {
            IController logicController = _controllers.Remove(_logicName);
            if (logicController is IDestroy destroy)
            {
                destroy.Destroy();
            }
        }

        private void Start() => _controllers.Start();

        private void Update() => _controllers.Update();

        private void FixedUpdate() => _controllers.FixedUpdate();

        private void LateUpdate() => _controllers.LateUpdate();

        private void OnDestroy() => _controllers.Destroy();
    }
}