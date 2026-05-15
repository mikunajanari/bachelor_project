using UnityEngine;
using UnityEngine.UI;

namespace cats.UI
{
    public class StatBarView : MonoBehaviour
    {
        [System.Serializable]
        public class SpriteLevels
        {
            public Sprite high;
            public Sprite mid;
            public Sprite low;
        }

        [Header("Sprites")]
        [SerializeField] private SpriteLevels _iconSprites;
        [SerializeField] private SpriteLevels _backgroundSprites;

        [Header("UI Elements")]
        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _backgroundImage;

        public void UpdateUI(float value)
        {
            Sprite iconSprite = GetSpriteByValue(value, _iconSprites);
            Sprite bgSprite = GetSpriteByValue(value, _backgroundSprites);

            if (_iconImage != null) _iconImage.sprite = iconSprite;
            if (_backgroundImage != null) _backgroundImage.sprite = bgSprite;
        }

        private Sprite GetSpriteByValue(float value, SpriteLevels sprites)
        {
            if (value >= 70f) return sprites.high;
            if (value >= 40f) return sprites.mid;
            return sprites.low;
        }
    }
}