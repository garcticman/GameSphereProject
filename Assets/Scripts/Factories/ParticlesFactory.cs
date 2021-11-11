using System.Collections.Generic;
using Settings;
using UnityEngine;

namespace Factories
{
    public class ParticlesFactory
    {
        private readonly ParticleViewsDataBase _particleViewsDataBase;
        private readonly Transform _root;

        private readonly Dictionary<ParticleType, Stack<ParticleSystem>> _particlesPool =
            new Dictionary<ParticleType, Stack<ParticleSystem>>();

        public ParticlesFactory(ParticleViewsDataBase particleViewsDataBase, Transform root)
        {
            _particleViewsDataBase = particleViewsDataBase;
            _root = root;
        }

        public ParticleSystem SpawnParticles(ParticleType particleType, Vector3 position, Quaternion rotation)
        {
            if (_particlesPool.TryGetValue(particleType, out var particleSystems)
                && particleSystems.Count > 0)
            {
                var particle = particleSystems.Pop();

                var transform = particle.transform;
                transform.position = position;
                transform.rotation = rotation;

                if (particle.main.playOnAwake)
                {
                    particle.Play();
                }

                return particle;
            }

            var particleSystemPrefab = _particleViewsDataBase.GetParticle(particleType);
            var particleSystemHolder = Object.Instantiate(particleSystemPrefab, position, rotation, _root);

            particleSystemHolder.OnParticleSystemStop += ()
                => OnParticleSystemPause(particleType, particleSystemHolder.ParticleSystem);

            return particleSystemHolder.ParticleSystem;
        }

        private void OnParticleSystemPause(ParticleType particleType, ParticleSystem particleSystem)
        {
            if (!_particlesPool.TryGetValue(particleType, out var particleSystems))
            {
                particleSystems = new Stack<ParticleSystem>();
                _particlesPool[particleType] = particleSystems;
            }

            particleSystem.Stop();
            particleSystems.Push(particleSystem);
        }
    }
}