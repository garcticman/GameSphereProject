using System.Linq;
using Factories;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "ParticleViewsDataBase", menuName = "ScriptableObjects/Settings/ParticleViewsDataBase")]
    public class ParticleViewsDataBase : ScriptableObject
    {
        [SerializeField] private ParticleViewRecord []particleViewRecords;
        
        public ParticleSystemHolder GetParticle(ParticleType particleType)
        {
            var particleRecord = particleViewRecords.FirstOrDefault(x => x.particleType == particleType);
            if (particleRecord != default) return particleRecord.particleSystemHolder;
            
            Debug.LogError($"Particle with type {particleType} not exist");
            return null;
        }
    }
}