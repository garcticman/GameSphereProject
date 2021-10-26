using System.Collections.Generic;
using System.Linq;
using Base;
using Factories;
using Services;
using Settings;
using UnityEngine;
using Views;

namespace Systems
{
    public class BalloonSystem : IUpdateSystem, IInitSystem, IDestroySystem
    {
        private readonly DifficultySettings _difficultySettings;
        private readonly BalloonFactory _balloonFactory;
        private readonly ScoreService _scoreService;

        private float _spawnDelay = 2;
        private float _currentTime;

        private readonly List<BalloonView> _existingBalloons = new List<BalloonView>();

        public BalloonSystem(BalloonFactory balloonFactory, DifficultySettings difficultySettings,
            ScoreService scoreService)
        {
            _balloonFactory = balloonFactory;
            _difficultySettings = difficultySettings;
            _scoreService = scoreService;
        }

        public void Init()
        {
            _scoreService.OnDifficultyChange += OnDifficultyChangeHandler;
        }

        public void Update()
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
            while (_existingBalloons.Count > 0)
            {
                _existingBalloons.First().Hide();
            }
            _scoreService.OnDifficultyChange -= OnDifficultyChangeHandler;
        }

        private void OnDifficultyChangeHandler()
        {
            _spawnDelay = _difficultySettings.GetDifficulty(_scoreService.CurrentDifficulty).spawnDelay;
        }

        private void CreateBalloon()
        {
            var balloonView = _balloonFactory.SpawnBalloon();
            balloonView.Show();
            
            balloonView.OnHide += OnBalloonHideHandler;
            _existingBalloons.Add(balloonView);
        }

        private void OnBalloonHideHandler(ViewBase balloon)
        {
            balloon.OnHide -= OnBalloonHideHandler;
            _existingBalloons.Remove((BalloonView) balloon);
        }
    }
}