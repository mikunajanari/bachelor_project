using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace cats.UI
{
    // Конкретная реализация UI представления
    public class CatUIView : MonoBehaviour, ICatUIView
    {
        [Header("Stat Bars")]
        [SerializeField] private StatBarView _hungerBar;
        [SerializeField] private StatBarView _moodBar;
        [SerializeField] private StatBarView _healthBar;
        
        [Header("Stat Text")]
        [SerializeField] private TMP_Text _hungerText;
        [SerializeField] private TMP_Text _moodText;
        [SerializeField] private TMP_Text _healthText;
        
        [Header("Cat Sprite (based on Mood)")]
        [SerializeField] private Sprite _happyCatSprite;
        [SerializeField] private Sprite _neutralCatSprite;
        [SerializeField] private Sprite _sadCatSprite;
        [SerializeField] private Image _catSpriteImage;

        public void UpdateUI(CatUIData data)
        {
            _hungerBar?.UpdateUI(data.Hunger);
            _moodBar?.UpdateUI(data.Mood);
            _healthBar?.UpdateUI(data.Health);

            UpdateStatText(_hungerText, data.Hunger);
            UpdateStatText(_moodText, data.Mood);
            UpdateStatText(_healthText, data.Health);

            UpdateCatSprite(data.Mood);
        }

        private void UpdateStatText(TMP_Text text, float value)
        {
            text.text = $"{value:F0}%";
        }

        private void UpdateCatSprite(float mood)
        {
            if (_catSpriteImage == null) return;
            
            if (mood >= 70f && _happyCatSprite != null)
                _catSpriteImage.sprite = _happyCatSprite;
            else if (mood >= 30f && _neutralCatSprite != null)
                _catSpriteImage.sprite = _neutralCatSprite;
            else if (_sadCatSprite != null)
                _catSpriteImage.sprite = _sadCatSprite;
        }
    }
}