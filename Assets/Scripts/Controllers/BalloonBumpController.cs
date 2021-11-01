using Base;
using Factories;
using Services;
using Settings;
using UnityEngine;

namespace Controllers
{
    public class BalloonBumpController : IController
    {
        private readonly ScoreService _scoreService;
        private readonly ParticlesFactory _particlesFactory;

        public BalloonBumpController(ScoreService scoreService, ParticlesFactory particlesFactory)
        {
            _scoreService = scoreService;
            _particlesFactory = particlesFactory;
        }

        public void BalloonBump(Vector3 position)
        {
            _scoreService.SubtractScore();
            _particlesFactory.SpawnParticles(ParticleType.BalloonBump, position, Quaternion.identity);
        }
    }
}