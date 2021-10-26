using Views;

namespace FiniteStateMachine.States
{
    public class GameState : IState
    {
        private readonly GameView _gameView;

        public bool IsActive { get; private set; }

        public GameState(GameView gameView)
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