using System;
using System.Collections.Generic;
using CardStacker.Core.StateMachine.Interfaces;

namespace CardStacker.Core.StateMachine
{
    public abstract class StateMachine
    {
        private protected readonly Dictionary<Type, IExitableState> States;
        private IExitableState _currentState;
        protected StateMachine()
        {
            DefaultState defaultState = new DefaultState();
            _currentState = defaultState;
            States = new Dictionary<Type, IExitableState> {[typeof(DefaultState)] = defaultState};
        }
        
        private protected void Enter<TState>() where TState : class, IState
        {
            TState state = ChangeState<TState>();
            state.Enter();
        }

        private protected void Enter<TState, TLoad>(TLoad load) where TState : class, ILoadingState<TLoad>
        {
            TState state = ChangeState<TState>();
            state.Enter(load);
        }

        private protected virtual TState ChangeState<TState>() where TState : class, IExitableState
        {
            _currentState.Exit();
            TState state = GetState<TState>();
            _currentState = state;
            return state;
        }

        private protected TState GetState<TState>() where TState : class, IExitableState => States[typeof(TState)] as TState;
    }
}