using System;
using Settings;
using Views;

namespace Services
{
    public class DifficultyService : IDisposable
    {
        public event Action OnDifficultyChange;

        private DifficultyType _currentDifficulty;

        public DifficultyType CurrentDifficulty => _menuView.IsEndlessModeOn ? _currentDifficulty : DifficultyType.VeryHard;
        
        private readonly ScoreService _scoreService;
        private readonly DifficultySettings _difficultySettings;
        private readonly MenuView _menuView;

        public DifficultyService(ScoreService scoreService, DifficultySettings difficultySettings, MenuView menuView)
        {
            _scoreService = scoreService;
            _difficultySettings = difficultySettings;
            _menuView = menuView;

            _scoreService.OnScoreChange += TryToChangeDifficulty;
        }

        public void Dispose()
        {
            _scoreService.OnScoreChange -= TryToChangeDifficulty;
        }

        private void TryToChangeDifficulty()
        {
            var newDifficulty = _difficultySettings.GetDifficultyByScore(_scoreService.Score);
            if (_currentDifficulty == newDifficulty) return;
            
            _currentDifficulty = newDifficulty;
            OnDifficultyChange?.Invoke();
        }
    }
}