using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using cats.UI;

namespace cats
{
    /// <summary>
    /// Handles drag-and-drop interactions for food items,
    /// allowing the player to feed the cat directly from the inventory.
    /// </summary>
    public class DraggableFoodSlot : MonoBehaviour,
        IPointerDownHandler,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _countText;
        [SerializeField] private GameObject _emptyOverlay;

        private FoodItem _food;
        private Canvas _canvas;
        private Camera _uiCamera;
        private AspectRatioFitter _fitter;

        public bool IsEmpty => _food == null;

        private const float IconScale = 0.7f;
        
        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
            _uiCamera = _canvas != null && _canvas.renderMode != RenderMode.ScreenSpaceOverlay
                ? _canvas.worldCamera
                : null;

            if (_iconImage != null)
            {
                RectTransform iconRect = _iconImage.GetComponent<RectTransform>();
                float padding = 50f;
                iconRect.anchorMin = Vector2.zero;
                iconRect.anchorMax = Vector2.one;
                iconRect.offsetMin = new Vector2(padding, padding);
                iconRect.offsetMax = new Vector2(-padding, -padding);

                iconRect.localScale = new Vector3(IconScale, IconScale, IconScale);

                _fitter = _iconImage.GetComponent<AspectRatioFitter>();
                if (_fitter == null)
                    _fitter = _iconImage.gameObject.AddComponent<AspectRatioFitter>();

                _fitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            }
        }

        /// <summary>
        /// Configures the slot to display a specific food item
        /// and its available quantity.
        /// </summary>
        public void Setup(FoodItem food, int count)
        {
            _food = food;

            if (_iconImage != null)
            {
                _iconImage.sprite = food.Icon;
                _iconImage.enabled = food.Icon != null;
                _iconImage.color = Color.white;

                if (_fitter != null && food.Icon != null)
                {
                    Rect r = food.Icon.rect;
                    _fitter.aspectRatio = r.width / r.height;
                }
            }

            if (_countText != null)
            {
                _countText.text = count > 1 ? count.ToString() : string.Empty;
                _countText.enabled = count > 1;
            }

            if (_emptyOverlay != null)
                _emptyOverlay.SetActive(false);
        }

        /// Resets the slot when no food item is available.
        public void SetEmpty()
        {
            _food = null;

            if (_iconImage != null)
            {
                _iconImage.sprite = null;
                _iconImage.enabled = false;
            }

            if (_fitter != null)
                _fitter.aspectRatio = 1f;

            if (_countText != null)
                _countText.enabled = false;

            if (_emptyOverlay != null)
                _emptyOverlay.SetActive(true);
        }

        public void OnPointerDown(PointerEventData eventData) { }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_food == null) return;
            if (DragHandler.Instance == null)
            {
                Debug.LogError("[DraggableFoodSlot] DragHandler is not found on the scene!");
                return;
            }
            DragHandler.Instance.BeginDrag(_food, this, _food.Icon);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (DragHandler.Instance == null || !DragHandler.Instance.IsDragging) return;
            DragHandler.Instance.MoveTo(eventData.position);

            var targets = FindObjectsByType<CatUIView>(FindObjectsSortMode.None);
            foreach (var t in targets)
                t.SetHighlight(t.ContainsScreenPoint(eventData.position, _uiCamera));
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var targets = FindObjectsByType<CatUIView>(FindObjectsSortMode.None);
            CatUIView hit = null;

            foreach (var t in targets)
            {
                if (t.ContainsScreenPoint(eventData.position, _uiCamera))
                    hit = t;
                else
                    t.SetHighlight(false);
            }

            FoodItem food = DragHandler.Instance != null
                ? DragHandler.Instance.EndDrag()
                : null;

            if (hit != null && food != null)
                hit.ReceiveDrop(food);
        }
    }
}