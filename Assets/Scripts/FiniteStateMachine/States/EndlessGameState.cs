using Views;

namespace FiniteStateMachine.States
{
    public class EndlessGameState : IState
    {
        private readonly GameView _gameView;

        public bool IsActive { get; private set; }

        public EndlessGameState(GameView gameView)
        {
            _gameView = gameView;
        }

        public void Activate()
        {
            IsActive = true;
            
            _gameView.Show();
        }

        public void Deactivate()
        {
            IsActive = false;
            
            _gameView.Hide();
        }
    }
}