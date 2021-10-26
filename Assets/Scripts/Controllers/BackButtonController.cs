using System;
using Base;
using Services;

namespace Controllers
{
    public class BackButtonController : IController
    {
        private readonly ScoreService _scoreService;
        public event Action OnBackButtonPressed;

        public BackButtonController(ScoreService scoreService)
        {
            _scoreService = scoreService;
        }
        
        public void BackButtonPressedInvoke()
        {
            _scoreService.ClearScore();
            OnBackButtonPressed?.Invoke();
        }
    }
}