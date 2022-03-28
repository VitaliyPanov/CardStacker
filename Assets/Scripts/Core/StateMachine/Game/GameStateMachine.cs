using CardStacker.Core.Services;
using CardStacker.General.Services;

namespace CardStacker.Core.StateMachine.Game
{
    public sealed class GameStateMachine : StateMachine
    {
        private readonly IBootstrapper _bootstrapper;

        public GameStateMachine(SceneLoader sceneLoader, LoadingScreen loadingCurtain, IGameFactory gameFactory,
            IBootstrapper bootstrapper)
        {
            _bootstrapper = bootstrapper;
            States[typeof(BootstrapState)] = new BootstrapState(this, sceneLoader);
            States[typeof(LoadSceneState)] = new LoadSceneState(this, sceneLoader, loadingCurtain, gameFactory);
            States[typeof(GameLoopState)] = new GameLoopState(this);
        }
        public void Start() => Enter<BootstrapState>();
        public void LoadScene(string sceneName) => Enter<LoadSceneState, string>(sceneName);
        public void EnterGameLoop()
        {
            if (_bootstrapper != null)
                _bootstrapper.Remove();
            Enter<GameLoopState>();
        }
    }
}