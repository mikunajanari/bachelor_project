using UnityEngine;

namespace cats.UI
{
    public class CatUIPresenter : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour _viewComponent; 
        
        private ICatUIView _view;
        private CatUIData _uiData;

        private void Start()
        {
            if (_viewComponent is ICatUIView view)
                _view = view;
            else
                Debug.LogError("View component doesn't implement ICatUIView");

            _uiData = new CatUIData();
            
            EventBus.Subscribe<TickEvent>(OnStatsChanged);
            EventBus.Subscribe<CatFedEvent>(OnStatsChanged);
        }

        private void OnStatsChanged<T>(T eventData) where T : struct
        {
            var cat = GetCatFromEvent(eventData);
            if (cat != null)
                UpdateUI(cat);
        }

        private Cat GetCatFromEvent<T>(T eventData)
        {
            if (eventData is TickEvent tick)
                return tick.Cat;
            if (eventData is CatFedEvent fed)
                return fed.Cat;
            return null;
        }

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