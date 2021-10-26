using System;

namespace FiniteStateMachine
{
    public abstract class TransitionBase : IDisposable
    {
        public IState StateFrom { get; private set; }
        public IState StateTo { get; private set; }

        public event Action<IState> OnTransitionComplete;

        public TransitionBase(IState stateFrom, IState stateTo)
        {
            StateFrom = stateFrom;
            StateTo = stateTo;
        }

        public virtual void Dispose()
        {
           
        }

        protected void DoTransition()
        {
            StateFrom.Deactivate();
            StateTo.Activate();
            OnTransitionComplete?.Invoke(StateTo);
        }
    }
}