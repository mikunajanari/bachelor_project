using UnityEngine;
using UnityEngine.UI;

namespace cats
{
    [RequireComponent(typeof(Button))]
    public class UIButton : MonoBehaviour
    {
        [SerializeField] private string _buttonId;

        public void SetButtonId(string id) => _buttonId = id;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            ButtonHandlerManager.Instance.HandleButtonClick(_buttonId);
        }
    }
}
