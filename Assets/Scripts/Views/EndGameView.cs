using Base;
using Controllers;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class EndGameView : ViewBase
    {
        [SerializeField] private Text score;
        [SerializeField] private Button backButton;
        
        private ScoreService _scoreService;

        public void SetData(ScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        public override void Show()
        {
            base.Show();
            
            backButton.onClick.AddListener(BackButtonClickHandler);
            score.text = _scoreService.Score.ToString();
        }

        public override void Hide()
        {
            base.Hide();
            backButton.onClick.RemoveListener(BackButtonClickHandler);
        }
        
        private void BackButtonClickHandler()
        {
            Interact<BackButtonController>(handler => handler.BackButtonPressedInvoke());
        }
    }
}