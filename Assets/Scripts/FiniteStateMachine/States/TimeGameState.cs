using Views;

namespace FiniteStateMachine.States
{
    public class TimeGameState : IState
    {
        private readonly GameView _gameView;
        private readonly TimerView _timerView;

        public bool IsActive { get; private set; }

        public TimeGameState(GameView gameView, TimerView timerView)
        {
            _gameView = gameView;
            _timerView = timerView;
        }

        public void Activate()
        {
            IsActive = true;
            
            _gameView.Show();
            _timerView.Show();
        }

        public void Deactivate()
        {
            IsActive = false;
            
            _gameView.Hide();
            _timerView.Hide();
        }
    }
}