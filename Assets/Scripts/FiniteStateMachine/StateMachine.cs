using System;
using System.Collections.Generic;

namespace FiniteStateMachine
{
    public class StateMachine : IDisposable
    {
        public event Action<IState> OnStateActivate;
        public event Action<IState> OnStateDeactivate;
        
        private IState _currentState;

        public IState CurrentState
        {
            get => _currentState;
            private set
            {
                OnStateDeactivate?.Invoke(_currentState);
                _currentState = value;
                OnStateActivate?.Invoke(_currentState);
            }
        }

        private readonly List<TransitionBase> _transitions = new List<TransitionBase>();
        private readonly List<IState> _states = new List<IState>();
        
        public void AddTransition(TransitionBase transition)
        {
            _transitions.Add(transition);
            
            transition.OnTransitionComplete += OnTransitionCompleteHandler;
        }

        public void AddState(IState state)
        {
            _states.Add(state);
        }

        public void ForceSet(IState startState)
        {
            foreach (var state in _states)
            {
                if (state == startState) continue;
                
                state.Deactivate();
            }
            
            startState.Activate();
            CurrentState = startState;
        }

        public void Dispose()
        {
            foreach (var transition in _transitions)
            {
                transition.OnTransitionComplete -= OnTransitionCompleteHandler;
                transition.Dispose();
            }
        }

        private void OnTransitionCompleteHandler(IState state)
        {
            CurrentState = state;
        }
    }
}