using CardStacker.Core.GameControllers;
using CardStacker.Core.StateMachine.Interfaces;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardStacker.Core.StateMachine.Game
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private CoreController _gameController;
        public GameLoopState(GameStateMachine stateMachine) => _stateMachine = stateMachine;

        public void Enter()
        {
            _gameController = Object.FindObjectOfType<CoreController>();
            _gameController.OnControllerDestroyEvent += Exit;
        }

        public void Exit()
        {
            _gameController.OnControllerDestroyEvent -= Exit;
            DOTween.Clear(true);
            SceneManager.LoadScene(SceneNames.LOADING);
        }
    }
}