using System.Linq;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "SoundsDataBase", menuName = "ScriptableObjects/Settings/SoundsDataBase")]
    public class SoundsDataBase : ScriptableObject
    {
        [SerializeField] private AudioClip[] audioClip;

        public AudioClip GetSound(string clipName)
        {
            var clip = audioClip.FirstOrDefault(x => x.name == clipName);
            if (clip != default) return clip;
            
            Debug.LogError($"clip with {name} not exist in {nameof(SoundsDataBase)}");
            return null;
        }
    }
}