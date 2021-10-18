using Base;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class StatusBarView : ViewBase
    {
        [SerializeField] private Text score;
        [SerializeField] private Text difficulty;
        
        private GameState _gameState;

        public void SetData(GameState gameState)
        {
            _gameState = gameState;
        }

        public override void Show()
        {
            base.Show();
            _gameState.OnScoreChange += Refresh;
        }

        public override void Hide()
        {
            base.Hide();
            _gameState.OnScoreChange -= Refresh;
        }

        public override void Refresh()
        {
            score.text = _gameState.Score.ToString();
            difficulty.text = _gameState.CurrentDifficulty.ToString();
        }
    }
}