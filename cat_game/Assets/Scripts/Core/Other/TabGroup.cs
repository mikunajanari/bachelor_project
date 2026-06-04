using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace cats
{
    public class TabGroup : MonoBehaviour
    {
        
        [SerializeField] private List<GameObject> _tabPanels;
        [SerializeField] private List<Button> _tabButtons;
        private List<ITabHandler> _tabHandlers = new List<ITabHandler>();

        private void Start()
        {
            InitializeTabs();
        }

        private void InitializeTabs()
        {
            if (_tabButtons == null || _tabPanels == null)
            {
                Debug.LogError($"[TabGroup] Lists not assigned in Inspector! ({gameObject.name})");
                return;
            }
            
            if (_tabButtons.Count != _tabPanels.Count)
            {
                Debug.LogError($"[TabGroup] TabButtons ({_tabButtons.Count}) != TabPanels ({_tabPanels.Count})");
                return;
            }
            
            for (int i = 0; i < _tabButtons.Count; i++)
            {
                var button = _tabButtons[i];
                var panel = _tabPanels[i];
                string buttonId = $"TabButton_{GetInstanceID()}_{i}";
                var handler = new TabButtonHandler(buttonId, panel, this, button);

                ButtonHandlerManager.Instance.Register(handler);
                _tabHandlers.Add(handler);

                var uiButton = button.GetComponent<UIButton>()
                    ?? button.gameObject.AddComponent<UIButton>();
                uiButton.SetButtonId(buttonId);

                if (i == 0) handler.ActivateTab();
                else handler.DisactivateTab();
            }
        }

        public void OnTabSelected(ITabHandler selectedHandler)
        {
            foreach (var handler in _tabHandlers)
            {
                if (handler == selectedHandler)
                    handler.ActivateTab();
                else
                    handler.DisactivateTab();
            }
        }
    }
}