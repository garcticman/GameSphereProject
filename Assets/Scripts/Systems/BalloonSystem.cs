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
    public class EndlessGameSystem : IUpdateSystem, IInitSystem, IDestroySystem
    {
        private readonly DifficultySettings _difficultySettings;
        private readonly BalloonFactory _balloonFactory;
        private readonly DifficultyService _difficultyService;

        private float _spawnDelay = 2;
        private float _nextBalloonSpawnTime;

        private readonly List<BalloonView> _existingBalloons = new List<BalloonView>();

        public EndlessGameSystem(BalloonFactory balloonFactory, DifficultySettings difficultySettings,
            DifficultyService difficultyService)
        {
            _balloonFactory = balloonFactory;
            _difficultySettings = difficultySettings;
            _difficultyService = difficultyService;
        }

        public void Init()
        {
            _difficultyService.OnDifficultyChange += SetActualDelay;
            SetActualDelay();
        }

        public void Update()
        {
            if (_nextBalloonSpawnTime < Time.time)
            {
                _nextBalloonSpawnTime = Time.time + _spawnDelay;
                CreateBalloon();
            }
        }

        public void Destroy()
        {
            while (_existingBalloons.Count > 0)
            {
                _existingBalloons.First().Hide();
            }
            _difficultyService.OnDifficultyChange -= SetActualDelay;
        }

        private void SetActualDelay()
        {
            _spawnDelay = _difficultySettings.GetDifficulty(_difficultyService.CurrentDifficulty).spawnDelay;
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