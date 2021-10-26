﻿namespace FiniteStateMachine
{
    public interface IState
    {
        public bool IsActive { get; }
        
        public void Activate();

        public void Deactivate();
    }
}