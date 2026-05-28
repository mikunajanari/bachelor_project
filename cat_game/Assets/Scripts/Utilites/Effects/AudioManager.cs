using System.Collections.Generic;
using UnityEngine;

namespace cats
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Music")]
        [SerializeField] private List<AudioSource> _musicSources;

        [Header("SFX")]
        [SerializeField] private List<SoundEffect> _sfxEffects;
        [SerializeField] private List<AudioClip> _sfxClips;
        [SerializeField] private int _sfxPoolSize = 6;

        private const string KeyMusic = "vol_music";
        private const string KeySfx   = "vol_sfx";

        private float _musicVolume;
        private float _sfxVolume;

        private List<AudioSource> _sfxPool;

        public float MusicVolume => _musicVolume;
        public float SfxVolume   => _sfxVolume;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _musicVolume = PlayerPrefs.GetFloat(KeyMusic, 1f);
            _sfxVolume   = PlayerPrefs.GetFloat(KeySfx,   1f);

            BuildPool();
            ApplyMusic();
        }

        private void BuildPool()
        {
            _sfxPool = new List<AudioSource>(_sfxPoolSize);
            for (int i = 0; i < _sfxPoolSize; i++)
            {
                var go = new GameObject($"SfxSource_{i}");
                go.transform.SetParent(transform);
                var src = go.AddComponent<AudioSource>();
                src.playOnAwake = false;
                src.loop = false;
                src.volume = _sfxVolume;
                _sfxPool.Add(src);
            }
        }

        public void SetMusicVolume(float value)
        {
            _musicVolume = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat(KeyMusic, _musicVolume);
            PlayerPrefs.Save();
            ApplyMusic();
        }

        public void SetSfxVolume(float value)
        {
            _sfxVolume = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat(KeySfx, _sfxVolume);
            PlayerPrefs.Save();
            ApplySfx();
        }

        public void PlaySfx(SoundEffect effect)
        {
            if (effect == null || effect.Clip == null) return;

            AudioSource src = GetFreeSource();
            src.volume = effect.Volume * _sfxVolume;
            src.pitch = effect.RandomPitch
                ? effect.Pitch + Random.Range(-effect.RandomPitchRange, effect.RandomPitchRange)
                : effect.Pitch;

            src.PlayOneShot(effect.Clip, 1f);
        }

        public void PlaySfx(AudioClip clip)
        {
            if (clip == null) return;
            AudioSource src = GetFreeSource();
            src.volume = _sfxVolume;
            src.pitch = 1f;
            src.PlayOneShot(clip, 1f);
        }

        public void PlaySfxByName(string effectName)
        {
            SoundEffect effect = _sfxEffects?.Find(e => e != null && e.name == effectName);
            if (effect != null) { PlaySfx(effect); return; }

            AudioClip clip = _sfxClips?.Find(c => c != null && c.name == effectName);
            if (clip != null) PlaySfx(clip);
        }

        public void RegisterMusicSource(AudioSource source)
        {
            if (source == null || _musicSources.Contains(source)) return;
            _musicSources.Add(source);
            source.volume = _musicVolume;
        }

        private void ApplyMusic()
        {
            foreach (var s in _musicSources)
                if (s != null) s.volume = _musicVolume;
        }

        private void ApplySfx()
        {
            foreach (var s in _sfxPool)
                if (s != null) s.volume = _sfxVolume;
        }

        private AudioSource GetFreeSource()
        {
            foreach (var s in _sfxPool)
                if (!s.isPlaying) return s;
            return _sfxPool[0];
        }
    }
}
