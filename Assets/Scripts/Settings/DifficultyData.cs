using System;

namespace Settings
{
    [Serializable]
    public struct DifficultyData
    {
        public DifficultyType difficultyType;
        public int scoreToReach;
        public float growingSpeed;
        public float spawnDelay;
    }
}