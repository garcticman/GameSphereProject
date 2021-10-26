using Controllers;

namespace FiniteStateMachine.Transitions
{
    public class MenuToGameTransition : TransitionBase
    {
        private readonly PlayButtonController _playButtonController;

        public MenuToGameTransition(IState stateFrom, IState stateTo, PlayButtonController playButtonController) : base(stateFrom, stateTo)
        {
            _playButtonController = playButtonController;

            _playButtonController.OnPlayPressed += DoTransition;
        }

        public override void Dispose()
        {
            _playButtonController.OnPlayPressed -= DoTransition;
        }
    }
}