namespace CardStacker.Core.StateMachine.Interfaces
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}