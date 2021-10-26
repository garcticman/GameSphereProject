namespace FiniteStateMachine.Transitions
{
    public class GameToEndScreenTransition : TransitionBase
    {
        private readonly IEndGameTransitionInvocator _endGameTransitionInvocator;

        public GameToEndScreenTransition(IState stateFrom, IState stateTo,
            IEndGameTransitionInvocator endGameTransitionInvocator) : base(stateFrom, stateTo)
        {
            _endGameTransitionInvocator = endGameTransitionInvocator;
            
            _endGameTransitionInvocator.OnEndGame += OnEndGameHandler;
        }

        public override void Dispose()
        {
            _endGameTransitionInvocator.OnEndGame -= OnEndGameHandler;
        }

        private void OnEndGameHandler()
        {
            DoTransition();
        }
    }
}