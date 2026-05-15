using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cats
{
    public class ButtonsClick : MonoBehaviour
    {
        [SerializeField] private SoundEffect _soundEffect;

        public void PlayClickSound()
        {
            if (_soundEffect != null)
            {
                SoundPlayerManager.Instance.PlaySound(_soundEffect);
            }
        }
    }
}
