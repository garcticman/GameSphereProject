using System;
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
            if (difficulties.All(x => x.difficultyType != difficultyType))
            {
                throw new Exception($"{nameof(DifficultySettings)} not contain difficulty with type: {difficultyType}");
            }

            return difficulties.First(x => x.difficultyType == difficultyType);
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