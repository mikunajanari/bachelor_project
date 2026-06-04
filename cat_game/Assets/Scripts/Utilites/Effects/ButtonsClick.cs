using UnityEngine;

namespace cats
{
    public class ButtonsClick : MonoBehaviour
    {
        [SerializeField] private SoundEffect _soundEffect;

        public void PlayClickSound()
        {
            if (_soundEffect != null)
                AudioManager.Instance.PlaySfx(_soundEffect);
        }
    }
}
