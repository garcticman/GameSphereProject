using Base;
using Core;

namespace Controllers
{
    public class BalloonBumpController : IController
    {
        private readonly GameState _gameState;

        public BalloonBumpController(GameState gameState)
        {
            _gameState = gameState;
        }

        public void OnInteract<T>(T interactData)
        {
            _gameState.SubtractScore();
        }

        public void OnInteract()
        {
            _gameState.SubtractScore();
        }
    }
}