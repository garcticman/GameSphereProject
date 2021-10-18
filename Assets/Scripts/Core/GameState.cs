using System;
using Settings;

namespace Core
{
    public class GameState
    {
        public event Action OnDifficultyChange;
        public event Action OnScoreChange;

        private readonly DifficultySettings _difficultySettings;
        
        private DifficultyType _currentDifficulty;
        
        public DifficultyType CurrentDifficulty => _currentDifficulty;

        private int _score;

        public int Score => _score;

        public GameState(DifficultySettings difficultySettings)
        {
            _difficultySettings = difficultySettings;
        }

        public void AddScore()
        {
            _score++;
            TryToChangeDifficulty();
            OnScoreChange?.Invoke();
        }

        public void SubtractScore()
        {
            _score--;
            TryToChangeDifficulty();
            OnScoreChange?.Invoke();
        }

        private void TryToChangeDifficulty()
        {
            var newDifficulty = _difficultySettings.GetDifficultyByScore(_score);
            if (_currentDifficulty == newDifficulty) return;
            
            _currentDifficulty = newDifficulty;
            OnDifficultyChange?.Invoke();
        }
    }
}