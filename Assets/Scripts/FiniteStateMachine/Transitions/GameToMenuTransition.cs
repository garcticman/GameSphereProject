using Controllers;

namespace FiniteStateMachine.Transitions
{
    public class GameToMenuTransition : TransitionBase
    {
        private readonly BackButtonController _buttonController;

        public GameToMenuTransition(IState stateFrom, IState stateTo, BackButtonController buttonController) 
            : base(stateFrom, stateTo)
        {
            _buttonController = buttonController;
            _buttonController.OnBackButtonPressed += DoTransition;
        }

        public override void Dispose()
        {
            _buttonController.OnBackButtonPressed -= DoTransition;
        }
    }
}