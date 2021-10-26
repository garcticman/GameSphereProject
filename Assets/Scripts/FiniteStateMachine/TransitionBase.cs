using System;

namespace FiniteStateMachine
{
    public abstract class TransitionBase : IDisposable
    {
        public IState StateFrom { get; }
        public IState StateTo { get; }

        public event Action<IState> OnTransitionComplete;

        private Func<bool> _condition;

        public TransitionBase(IState stateFrom, IState stateTo)
        {
            StateFrom = stateFrom;
            StateTo = stateTo;
        }

        public void SetCondition(Func<bool> condition)
        {
            _condition = condition;
        }

        public virtual void Dispose()
        {
           
        }

        protected void DoTransition()
        {
            if (_condition != null && !_condition.Invoke())
                return;

            StateFrom.Deactivate();
            StateTo.Activate();
            OnTransitionComplete?.Invoke(StateTo);
        }
    }
}