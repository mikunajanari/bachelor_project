using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace cats
{
    public class UniversalButton : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private ButtonsClick _buttonsClick;
        [SerializeField] private ButtonActionType _actionType;
        [SerializeField] private float _panelAnimationDuration = 0.5f;

        [Header("Panel Settings")]
        [SerializeField] private GameObject _panelToControl; // For Open/Close Panel type
        [SerializeField] private bool _animatePanel = true;

        [Header("Button Settings")] 
        [SerializeField] private bool _blockDuringAnimation = true;

        private Button _buttonComponent;
        private bool _isAnimating;

        private void Awake()
        {
            _buttonComponent = GetComponent<Button>();
        }

        public void OnButtonClicked()
        {
            if (_isAnimating) return;

            PlayPunch();
            HandleButtonAction();
        }

        
        private void HandleButtonAction()
        {
            switch (_actionType)
            {
                case ButtonActionType.OpenPanel:
                    SetPanelState(true);
                    break;

                case ButtonActionType.ClosePanel:
                    SetPanelState(false);
                    break;

                case ButtonActionType.TogglePanel:
                    if (_panelToControl != null)
                    {
                        SetPanelState(!_panelToControl.activeSelf);
                    }
                    break;

                case ButtonActionType.PlaySoundOnly:
                    PlayCustomSound();
                    break;

                case ButtonActionType.CustomAction:
                    GetComponent<IButtonAction>()?.ExecuteAction();
                    break;

                case ButtonActionType.None:
                default:
                    // Do nothing, just animation
                    break;
            }
        }

        private void SetPanelState(bool state)
        {
            if (_panelToControl == null) return;
            
            if (_blockDuringAnimation && _buttonComponent != null)
                _buttonComponent.interactable = false;

            _isAnimating = true;

            if (_animatePanel)
            {
                if (state)
                {
                    _panelToControl.SetActive(true);
                    _panelToControl.transform.localScale = Vector3.zero;
                    _panelToControl.transform.DOScale(Vector3.one, _panelAnimationDuration)
                        .SetEase(Ease.OutBack)
                        .OnComplete(() => FinishAnimation()).Play();
                }
                else
                {
                    if (!_panelToControl.activeSelf)
                        _panelToControl.SetActive(true);

                    _panelToControl.transform.DOScale(Vector3.zero, _panelAnimationDuration)
                        .SetEase(Ease.InBack)
                        .OnComplete(() =>
                        {
                            _panelToControl.SetActive(false);
                            FinishAnimation();
                        }).Play();
                }
            }
            else
            {
                // Without Animation
                _panelToControl.SetActive(state);
                FinishAnimation();
            }

            PlayCustomSound();
        }

        private void FinishAnimation()
        {
            _isAnimating = false;
            if (_blockDuringAnimation && _buttonComponent != null)
                _buttonComponent.interactable = true;
        }

        private void PlayCustomSound()
        {
            _buttonsClick.PlayClickSound();
        }

        public void PlayPunch()
        {
            if (_rect == null) return;
            
            _rect.DOKill();
            _rect.localScale = Vector3.one;

            _rect.DOPunchScale(
                new Vector3(0.2f, -0.1f, 0f),
                0.3f,
                8,
                0.5f
            ).SetEase(Ease.OutQuad).Play();
        }
    }
}