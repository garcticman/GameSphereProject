using Base;
using Core;
using Factories;
using Services;
using Settings;
using UnityEngine;

namespace Controllers
{
    public class BalloonTouchController : IController
    {
        private readonly ScoreService _scoreService;
        private readonly ParticlesFactory _particlesFactory;
        private readonly SoundManager _soundManager;

        public BalloonTouchController(ScoreService scoreService, ParticlesFactory particlesFactory, SoundManager soundManager)
        {
            _scoreService = scoreService;
            _particlesFactory = particlesFactory;
            _soundManager = soundManager;
        }
        
        public void BalloonTouch(Vector3 position)
        {
            _scoreService.AddScore();
            _particlesFactory.SpawnParticles(ParticleType.BalloonTouch, position, Quaternion.identity);
            _soundManager.PlaySound(SoundNames.Pop, Random.Range(0.2f, 0.5f));
        }
    }
}