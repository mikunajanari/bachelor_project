using UnityEngine;
using UnityEngine.UI;

namespace cats
{
    public class DragHandler : MonoBehaviour
    {
        public static DragHandler Instance { get; private set; }

        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _ghostImage;

        private RectTransform _ghostRect;
        private Camera _uiCamera;
        private FoodItem _draggedFood;
        private DraggableFoodSlot _sourceSlot;

        public bool IsDragging => _draggedFood != null;
        public FoodItem DraggedFood => _draggedFood;

        private void Awake()
        {
            Instance = this;
            _ghostRect = _ghostImage.GetComponent<RectTransform>();
            _ghostImage.gameObject.SetActive(false);

            _uiCamera = _canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : _canvas.worldCamera;
        }

        public void BeginDrag(FoodItem food, DraggableFoodSlot source, Sprite icon)
        {
            _draggedFood = food;
            _sourceSlot = source;

            _ghostImage.sprite = icon;
            _ghostImage.gameObject.SetActive(true);

            EventBus.Publish(new DragStartedEvent { FoodItem = food });
        }

        public void MoveTo(Vector2 screenPosition)
        {
            if (!IsDragging) return;

            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                _canvas.transform as RectTransform,
                screenPosition,
                _uiCamera,
                out Vector3 worldPos);

            _ghostRect.position = worldPos;
        }

        public FoodItem EndDrag()
        {
            FoodItem food = _draggedFood;

            _draggedFood = null;
            _sourceSlot = null;
            _ghostImage.gameObject.SetActive(false);

            EventBus.Publish(new DragEndedEvent());

            return food;
        }
    }
}
