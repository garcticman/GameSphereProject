using System.Linq;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "DifficultySettings", menuName = "ScriptableObjects/Settings/DifficultySettings")]
    public class DifficultySettings : ScriptableObject
    {
        [SerializeField] private DifficultyData[] difficulties;

        public DifficultyData GetDifficulty(DifficultyType difficultyType)
        {
            var difficulty = GetDifficultyOrDefault(difficultyType);
            if (difficulty != null) return difficulty;
            
            Debug.Log($"Difficulty with {difficultyType} not exist");
            return new DifficultyData
            {
                difficultyType = DifficultyType.Easy,
                growingSpeed = 2,
                spawnDelay = 1,
                scoreToReach = 0
            };
        }

        private DifficultyData GetDifficultyOrDefault(DifficultyType difficultyType)
        {
            var difficulty = difficulties.FirstOrDefault(x => x.difficultyType == difficultyType);
            return difficulty;
        }

        public DifficultyType GetDifficultyByScore(int score)
        {
            var difficultyType = DifficultyType.Easy;
            var minScore = 0;
            
            foreach (var difficulty in difficulties)
            {
                if (difficulty.scoreToReach > score || minScore >= difficulty.scoreToReach) 
                    continue;

                minScore = difficulty.scoreToReach;
                difficultyType = difficulty.difficultyType;
            }

            return difficultyType;
        }
    }
}