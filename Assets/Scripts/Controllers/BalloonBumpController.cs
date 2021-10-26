using Base;
using Services;

namespace Controllers
{
    public class BalloonBumpController : IController
    {
        private readonly ScoreService _scoreService;

        public BalloonBumpController(ScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        public void BalloonBump()
        {
            _scoreService.SubtractScore();
        }
    }
}