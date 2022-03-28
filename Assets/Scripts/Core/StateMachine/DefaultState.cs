using CardStacker.Core.StateMachine.Interfaces;

namespace CardStacker.Core.StateMachine
{
    public sealed class DefaultState : IState, IUpdateCameraState
    {
        public void Exit(){}
        public void Enter(){}

        public void Update(){}
    }
}