using Base;
using Core;
using Fabrics;
using Settings;
using UnityEngine;

namespace Systems
{
    public class BalloonSpawnSystem : ISystem
    {
        private DifficultySettings _difficultySettings;
        private readonly BalloonFactory _balloonFactory;
        private readonly GameState _gameState;
        
        private float _spawnDelay = 2;
        private float _currentTime;

        public BalloonSpawnSystem(BalloonFactory balloonFactory, GameState gameState, DifficultySettings difficultySettings)
        {
            _balloonFactory = balloonFactory;
            _gameState = gameState;
            _difficultySettings = difficultySettings;
        }

        public void Init()
        {
            _gameState.OnDifficultyChange += OnDifficultyChangeHandler;
        }

        public void Execute()
        {
            _currentTime += Time.deltaTime;
            if (_currentTime > _spawnDelay)
            {
                _currentTime = 0;
                CreateBalloon();
            }
        }

        public void Destroy()
        {
            _gameState.OnDifficultyChange -= OnDifficultyChangeHandler;
        }

        private void OnDifficultyChangeHandler()
        {
            _spawnDelay = _difficultySettings.GetDifficulty(_gameState.CurrentDifficulty).spawnDelay;
        }

        private void CreateBalloon()
        {
            var balloonView = _balloonFactory.SpawnBalloon();
            balloonView.Show();
        }
    }
}