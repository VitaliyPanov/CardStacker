using CardStacker.Core.GameControllers;
using CardStacker.Core.Services;
using CardStacker.Core.Services.Input;
using CardStacker.Core.Services.Pooling;
using CardStacker.GameLogic;
using CardStacker.General.Controllers;
using CardStacker.General.Services;
using CardStacker.General.Services.Input;
using UnityEngine;
using Zenject;

namespace CardStacker.Core
{
    internal sealed class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindGeneralServices();
            BindControllers();
        }

        private void BindGeneralServices()
        {
            Container.Bind<IGameFactory>().To<GameFactory>().AsSingle();
            Container.Bind<IDataService>().To<DataService>().AsSingle();
            Container.Bind<IPoolService>().To<PoolService>().AsSingle();
            Container.Bind<IViewService>().To<ViewService>().AsSingle();
            Container.Bind<IInputService>().To<InputService>().AsSingle();
        }

        private void BindControllers()
        {
            Container.Bind<IControllersMediator>().To<ControllersMediator>()
                .FromInstance(new GameObject("[MEDIATOR]").AddComponent<ControllersMediator>());
            Container.Bind<ILogicController>().To<GameLogicController>().AsSingle();
            Container.Bind<ICameraController>().To<CameraController>().AsSingle();
            Container.Bind<IUIController>().To<UIController>().AsSingle();
        }
    }
}