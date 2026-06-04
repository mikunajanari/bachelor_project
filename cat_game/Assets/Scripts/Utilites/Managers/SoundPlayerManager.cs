using UnityEngine;

namespace cats
{
    public class SoundPlayerManager : MonoBehaviour
    {
        public static SoundPlayerManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else Destroy(gameObject);
        }

        public void PlaySound(SoundEffect soundEffect,
            float volumeOverride = -1f, float pitchOverride = -1f)
        {
            if (soundEffect == null || soundEffect.Clip == null) return;

            GameObject soundGo = new GameObject("TempAudioSource");

            AudioSource audioSource = soundGo.AddComponent<AudioSource>();
            audioSource.clip = soundEffect.Clip;

            //Settings Parameters
            audioSource.volume = volumeOverride >= 0 ? volumeOverride : soundEffect.Volume;
            audioSource.pitch = pitchOverride >= 0 ? pitchOverride : soundEffect.Pitch;

            if (soundEffect.RandomPitch)
            {
                audioSource.pitch += Random.Range(-soundEffect.RandomPitchRange, soundEffect.RandomPitchRange);
            }

            audioSource.Play();
            Destroy(audioSource.gameObject, audioSource.clip.length + 0.1f);
        }
    }
}