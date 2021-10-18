using Base;
using Core;

namespace Controllers
{
    public class BalloonTouchController : IController
    {
        private readonly GameState _gameState;

        public BalloonTouchController(GameState gameState)
        {
            _gameState = gameState;
        }

        public void OnInteract<T>(T interactData)
        {
            _gameState.AddScore();
        }

        public void OnInteract()
        {
            _gameState.AddScore();
        }
    }
}