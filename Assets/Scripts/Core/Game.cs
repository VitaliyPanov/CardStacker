using CardStacker.Core.Services;
using CardStacker.Core.StateMachine.Game;
using CardStacker.General.Services;

namespace CardStacker.Core
{
    internal sealed class Game
    {
        private readonly GameStateMachine _stateMachine;

        public Game(LoadingScreen loadingCurtain, IGameFactory gameFactory, IBootstrapper bootstrapper) =>
            _stateMachine = new GameStateMachine(new SceneLoader(), loadingCurtain, gameFactory, bootstrapper);

        public void Start() => _stateMachine.Start();
    }
}