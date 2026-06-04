using UnityEngine;

namespace cats.UI
{
    /// <summary>
    /// Connects the cat model with the user interface and keeps displayed
    /// data synchronized with gameplay events.
    /// </summary>
    public class CatUIPresenter : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour _viewComponent; 
        
        private ICatUIView _view;
        private CatUIData _uiData;

        private void Start()
        {
            // Ensures the assigned component can receive UI updates.
            if (_viewComponent is ICatUIView view)
                _view = view;
            else
                Debug.LogError("View component doesn't implement ICatUIView");

            _uiData = new CatUIData();
            
            // Refreshes the interface whenever the cat state changes.
            EventBus.Subscribe<TickEvent>(OnStatsChanged);
            EventBus.Subscribe<CatFedEvent>(OnStatsChanged);
        }

        private void OnStatsChanged<T>(T eventData) where T : struct
        {
            var cat = GetCatFromEvent(eventData);
            if (cat != null)
                UpdateUI(cat);
        }

        /// <summary>
        /// Extracts the cat instance from the event data.
        /// </summary>
        private Cat GetCatFromEvent<T>(T eventData)
        {
            if (eventData is TickEvent tick)
                return tick.Cat;
            if (eventData is CatFedEvent fed)
                return fed.Cat;
            return null;
        }

        /// <summary>
        /// Converts model data into a UI-friendly format and forwards it to the view.
        /// </summary>
        private void UpdateUI(Cat cat)
        {
            _uiData.UpdateStats(cat);
            _view?.UpdateUI(_uiData);
        }

        private void OnDestroy()
        {
        }
    }
}