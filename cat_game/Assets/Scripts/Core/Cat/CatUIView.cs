using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace cats.UI
{
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
        
        [Header("Drag & Drop")]
        [SerializeField] private Sprite _dragHoverSprite;
        [SerializeField] private GameObject _highlightOverlay;
        
        [Header("Feed")]
        [SerializeField] private CatController _catController;
        [SerializeField] private FeedingFeedback _feedback;

        private Sprite _normalSprite;
        private bool _isDragActive;
        private bool _useTemporarySprite;
        private Sprite _originalSprite;
        private RectTransform _rect;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            
            if (_catSpriteImage != null)
                _normalSprite = _catSpriteImage.sprite;
            
            // Настраиваем зону для дропа
            Image zoneImage = GetComponent<Image>();
            if (zoneImage != null && zoneImage != _catSpriteImage)
            {
                zoneImage.color = Color.clear;
                zoneImage.raycastTarget = true;
            }
            else if (_catSpriteImage != null)
            {
                _catSpriteImage.raycastTarget = true;
            }
        }

        private void Start()
        {
            ShowNormalSprite();
            SetHighlightVisuals(false);
        }

        private void OnEnable()
        {
            EventBus.Subscribe<DragStartedEvent>(OnDragStarted);
            EventBus.Subscribe<DragEndedEvent>(OnDragEnded);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<DragStartedEvent>(OnDragStarted);
            EventBus.Unsubscribe<DragEndedEvent>(OnDragEnded);
        }

        private void OnDragStarted(DragStartedEvent _)
        {
            _isDragActive = true;
        }

        private void OnDragEnded(DragEndedEvent _)
        {
            _isDragActive = false;
            ShowNormalSprite();
            SetHighlightVisuals(false);
        }

        public void UpdateUI(CatUIData data)
        {
            _hungerBar?.UpdateUI(data.Hunger);
            _moodBar?.UpdateUI(data.Mood);
            _healthBar?.UpdateUI(data.Health);

            UpdateStatText(_hungerText, data.Hunger);
            UpdateStatText(_moodText, data.Mood);
            UpdateStatText(_healthText, data.Health);

            if (!_useTemporarySprite)
            {
                UpdateCatSpriteByMood(data.Mood);
            }
        }

        public bool ContainsScreenPoint(Vector2 screenPoint, Camera cam)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(_rect, screenPoint, cam);
        }

        public void ReceiveDrop(FoodItem food)
        {
            if (_catController == null) return;
            _catController.FeedWithItem(food);
            _feedback?.Play(food);
            ShowNormalSprite();
            SetHighlightVisuals(false);
        }

        public void SetHighlight(bool active)
        {
            SetHighlightVisuals(active);

            if (_isDragActive)
            {
                if (active)
                    ShowHoverSprite();
                else
                    ShowNormalSprite();
            }
        }

        private void SetHighlightVisuals(bool active)
        {
            if (_highlightOverlay != null) 
                _highlightOverlay.SetActive(active);

        }

        private void ShowHoverSprite()
        {
            if (_catSpriteImage == null || _dragHoverSprite == null) return;
            
            if (!_useTemporarySprite)
            {
                _originalSprite = _catSpriteImage.sprite;
                _useTemporarySprite = true;
            }
            
            _catSpriteImage.sprite = _dragHoverSprite;

        }

        private void ShowNormalSprite()
        {
            if (_catSpriteImage == null) return;
            
            if (_useTemporarySprite && _originalSprite != null)
            {
                _catSpriteImage.sprite = _originalSprite;
                _useTemporarySprite = false;
            }
            else if (_normalSprite != null && !_useTemporarySprite)
            {
                _catSpriteImage.sprite = _normalSprite;
            }

        }

        private void UpdateCatSpriteByMood(float mood)
        {
            if (_catSpriteImage == null) return;
            
            if (mood >= 70f && _happyCatSprite != null)
                _catSpriteImage.sprite = _happyCatSprite;
            else if (mood >= 30f && _neutralCatSprite != null)
                _catSpriteImage.sprite = _neutralCatSprite;
            else if (_sadCatSprite != null)
                _catSpriteImage.sprite = _sadCatSprite;

        }

        private void UpdateStatText(TMP_Text text, float value)
        {
            if (text != null)
                text.text = $"{value:F0}%";
        }
    }
}