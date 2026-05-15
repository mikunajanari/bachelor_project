using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace cats
{
    public class TabButtonHandler : ITabHandler
    {
        public string ButtonId { get; private set; }
        public GameObject TabPanel { get; private set; }
        
        private TabGroup _tabGroup;
        private RectTransform _buttonTransform;
        private Vector2 _originalPosition;
        private const float MoveHeight = 15f;
        private const float AnimationDuration = 0.4f;

        public TabButtonHandler(string buttonId, GameObject tabPanel, TabGroup tabGroup, Button button)
        {
            ButtonId = buttonId;
            TabPanel = tabPanel;
            _tabGroup = tabGroup;
            
            _buttonTransform = button.GetComponent<RectTransform>();
            _originalPosition = _buttonTransform.anchoredPosition;
        }

        public void Handle()
        {
            _tabGroup.OnTabSelected(this);
        }

        public void ActivateTab()
        {
            TabPanel.SetActive(true);

            _buttonTransform.DOKill();
            _buttonTransform.DOAnchorPosY(
                _originalPosition.y + MoveHeight,
                AnimationDuration
            )
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                _buttonTransform.DOShakePosition(
                    0.1f,
                    new Vector3(0, 2f, 0),
                    vibrato: 10
                );
            }).Play();
        }

        public void DisactivateTab()
        {
            TabPanel.SetActive(false);

            _buttonTransform.DOKill();
            _buttonTransform.DOAnchorPos(
                _originalPosition,
                AnimationDuration * 0.3f
            )
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                _buttonTransform.anchoredPosition = _originalPosition;
            }).Play();
        }
    }
}
