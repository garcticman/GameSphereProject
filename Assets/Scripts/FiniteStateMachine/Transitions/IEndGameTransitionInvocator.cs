using System;

namespace FiniteStateMachine.Transitions
{
    public interface IEndGameTransitionInvocator
    {
        event Action OnEndGame;
    }
}