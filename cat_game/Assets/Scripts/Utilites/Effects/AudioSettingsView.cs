using UnityEngine;
using UnityEngine.UI;

namespace cats
{
    public class AudioSettingsView : MonoBehaviour
    {
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;

        private void Start()
        {
            _musicSlider.value = AudioManager.Instance.MusicVolume;
            _sfxSlider.value   = AudioManager.Instance.SfxVolume;

            _musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
            _sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSfxVolume);
        }

        private void OnDestroy()
        {
            _musicSlider.onValueChanged.RemoveListener(AudioManager.Instance.SetMusicVolume);
            _sfxSlider.onValueChanged.RemoveListener(AudioManager.Instance.SetSfxVolume);
        }
    }
}
