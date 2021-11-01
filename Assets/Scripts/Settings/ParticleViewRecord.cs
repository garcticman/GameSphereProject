using System;
using Factories;

namespace Settings
{
    [Serializable]
    public class ParticleViewRecord
    {
        public ParticleType particleType;
        public ParticleSystemHolder particleSystemHolder;
    }
}