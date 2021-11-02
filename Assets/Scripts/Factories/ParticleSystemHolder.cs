using System;
using UnityEngine;

namespace Factories
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleSystemHolder : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particleSystem;

        public ParticleSystem ParticleSystem => particleSystem;

        public event Action OnParticleSystemPause;

        private void OnValidate()
        {
            particleSystem = GetComponent<ParticleSystem>();
        }

        private void OnParticleSystemStopped()
        {
            OnParticleSystemPause?.Invoke();
        }
    }
}