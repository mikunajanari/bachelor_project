using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cats
{
    [CreateAssetMenu(fileName = "NewSoundEffect", menuName = "CoonGame/Audio/Sound Effect")]
    public class SoundEffect : ScriptableObject
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField][Range(0, 1)] private float _volume = 1f;
        [SerializeField][Range(-3, 3)] private float _pitch = 1f;
        [SerializeField][Range(0, 0.5f)] private float _pitchRandomRange = 0.1f;
        [SerializeField] private bool _randomPitch = false;

        public AudioClip Clip => _clip;
        public float Volume => _volume;
        public float Pitch => _pitch;
        public float RandomPitchRange => _pitchRandomRange;
        public bool RandomPitch => _randomPitch;
    }
}
