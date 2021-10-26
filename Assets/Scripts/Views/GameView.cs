using Base;
using Controllers;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class GameView : ViewBase
    {
        [SerializeField] private Text score;
        [SerializeField] private Text difficulty;
        [SerializeField] private Button backButton;
        
        private ScoreService _scoreService;
        private DifficultyService _difficultyService;

        public void SetData(ScoreService scoreService, DifficultyService difficultyService)
        {
            _scoreService = scoreService;
            _difficultyService = difficultyService;
        }

        public override void Show()
        {
            base.Show();
            backButton.onClick.AddListener(BackButtonClickHandler);
            _scoreService.OnScoreChange += Refresh;
            _difficultyService.OnDifficultyChange += Refresh;
            Refresh();
        }

        public override void Hide()
        {
            base.Hide();
            backButton.onClick.RemoveListener(BackButtonClickHandler);
            if (_scoreService != null)
            {
                _scoreService.OnScoreChange -= Refresh;
            }
            if (_difficultyService != null)
            {
                _difficultyService.OnDifficultyChange -= Refresh;
            }
        }

        private void BackButtonClickHandler()
        {
            Interact<BackButtonController>(handler => handler.BackButtonPressedInvoke());
        }

        private void Refresh()
        {
            score.text = _scoreService.Score.ToString();
            difficulty.text = _difficultyService.CurrentDifficulty.ToString();
        }
    }
}