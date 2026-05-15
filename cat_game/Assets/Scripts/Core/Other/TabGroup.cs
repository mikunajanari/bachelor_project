using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace cats
{
    public class TabGroup : MonoBehaviour
    {
        public static TabGroup Instance { get; private set; }
        
        [SerializeField] private List<GameObject> _tabPanels;
        [SerializeField] private List<Button> _tabButtons;
        private List<ITabHandler> _tabHandlers = new List<ITabHandler>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            InitializeTabs();
            //awdawd
        }

        private void InitializeTabs()
        {
            for (int i = 0; i < _tabButtons.Count; i++)
            {
                var button = _tabButtons[i];
                var panel = _tabPanels[i];
                
                string buttonId = $"TabButton_{i}";
                var handler = new TabButtonHandler(buttonId, panel, this, button);
                
                ButtonHandlerManager.Instance.Register(handler);
                _tabHandlers.Add(handler);
                
                var uiButton = button.GetComponent<UIButton>() ?? button.gameObject.AddComponent<UIButton>();
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
