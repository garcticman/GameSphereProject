using Views;

namespace FiniteStateMachine.States
{
    public class EndGameState : IState
    {
        private readonly EndGameView _endGameView;

        private bool _isActive;
        public bool IsActive => _isActive;

        public EndGameState(EndGameView endGameView)
        {
            _endGameView = endGameView;
        }
        
        public void Activate()
        {
            _isActive = true;
            
            _endGameView.Show();
        }

        public void Deactivate()
        {
            _isActive = false;
            
            _endGameView.Hide();
        }
    }
}