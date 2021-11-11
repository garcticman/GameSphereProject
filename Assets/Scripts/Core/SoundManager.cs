using Settings;
using UnityEditor;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundsDataBase soundsDataBase;
        [SerializeField] private AudioSource audioSource;

        private void OnValidate()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(string clipName, float volume = 1.0f)
        {
            var sound = soundsDataBase.GetSound(clipName);
            audioSource.PlayOneShot(sound, volume);
        }

        public void PlayMusic(string clipName, float volume = 1.0f)
        {
            var sound = soundsDataBase.GetSound(clipName);
            audioSource.clip = sound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
