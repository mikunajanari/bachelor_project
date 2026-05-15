using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cats
{
    public class ButtonHandlerManager : MonoBehaviour
    {
        public static ButtonHandlerManager Instance { get; private set; }

        private readonly Dictionary<string, IButtonHandler> _handlers = new Dictionary<string, IButtonHandler>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            RegisterHandlers();
        }

        private void RegisterHandlers()
        {
            // Register(new MenuButtonHandler());
            // Register(new NoButtonHandler());
            // Register(new YesButtonHandler());
        }

        public void Register(IButtonHandler buttonHandler)
        {
            if (!_handlers.ContainsKey(buttonHandler.ButtonId))
            {
                _handlers.Add(buttonHandler.ButtonId, buttonHandler);
            }
        }

        public void HandleButtonClick(string buttonId)
        {
            if (_handlers.TryGetValue(buttonId, out var handler))
            {
                handler.Handle();
            }
            else
            {
                Debug.LogError($"No handler for: {buttonId}");
            }
        }
    }
}
