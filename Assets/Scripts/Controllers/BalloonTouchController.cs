using Base;
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

        public BalloonTouchController(ScoreService scoreService, ParticlesFactory particlesFactory)
        {
            _scoreService = scoreService;
            _particlesFactory = particlesFactory;
        }
        
        public void BalloonTouch(Vector3 position)
        {
            _scoreService.AddScore();
            _particlesFactory.SpawnParticles(ParticleType.BalloonTouch, position, Quaternion.identity);
        }
    }
}