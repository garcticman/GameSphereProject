using System;
using FiniteStateMachine.Transitions;

namespace Services
{
    public class ScoreService : IEndGameTransitionInvocator
    {
        public event Action OnScoreChange;
        public event Action OnEndGame;

        private int _score;
        public int Score => _score;


        public void AddScore()
        {
            _score++;
            OnScoreChange?.Invoke();
        }

        public void SubtractScore()
        {
            _score--;
            OnScoreChange?.Invoke();

            if (_score <= 0)
            {
                OnEndGame?.Invoke();
            }
        }

        public void ClearScore()
        {
            _score = 0;
            OnScoreChange?.Invoke();
        }
    }
}