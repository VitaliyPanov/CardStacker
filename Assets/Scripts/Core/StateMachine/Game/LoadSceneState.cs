using CardStacker.Core.Services;
using CardStacker.Core.StateMachine.Interfaces;
using CardStacker.General.Services;

namespace CardStacker.Core.StateMachine.Game
{
    public sealed class LoadSceneState : ILoadingState<string>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingScreen _loadingCurtain;
        private readonly IGameFactory _gameFactory;

        public LoadSceneState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingScreen loadingCurtain,
            IGameFactory gameFactory)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
        }

        public void Enter(string scene)
        {
            if (_loadingCurtain != null) _loadingCurtain.Show();
            _sceneLoader.Load(scene, OnSceneLoaded);
        }

        private void OnSceneLoaded()
        {
            _gameFactory.LoadData();
            _gameFactory.CreateGameController();
            _stateMachine.EnterGameLoop();
        }

        public void Exit()
        {
            if (_loadingCurtain != null) _loadingCurtain.Hide();
        }
    }
}