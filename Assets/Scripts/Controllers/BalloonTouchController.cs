using Base;
using Services;

namespace Controllers
{
    public class BalloonTouchController : IController
    {
        private readonly ScoreService _scoreService;

        public BalloonTouchController(ScoreService scoreService)
        {
            _scoreService = scoreService;
        }
        
        public void BalloonTouch()
        {
            _scoreService.AddScore();
        }
    }
}