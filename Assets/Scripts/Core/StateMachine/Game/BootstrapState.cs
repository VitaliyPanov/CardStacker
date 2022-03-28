using CardStacker.Core.Services;
using CardStacker.Core.StateMachine.Interfaces;

namespace CardStacker.Core.StateMachine.Game
{
    public sealed class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter() => _sceneLoader.Load(SceneNames.LOADING, EnterSceneLoadingState);

        private void EnterSceneLoadingState() => _stateMachine.LoadScene(SceneNames.MAIN);
        public void Exit() {}
    }
}