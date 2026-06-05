using TMPro;
using UnityEngine;

namespace cats
{
    public class CurrencyDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _coinsText;

        private void HandlePurchase(ItemPurchasedEvent _) => Refresh();
        private void HandleCurrencyChanged(CurrencyChangedEvent _) => Refresh();

        private void Awake()
        {
            CurrencyWallet.Instance.Reload();
            Refresh();
        }

        private void Start()
        {
            Refresh();
        }

        private void OnEnable()
        {
            CurrencyWallet.Instance.Reload();
            Refresh();
            EventBus.Subscribe<ItemPurchasedEvent>(HandlePurchase);
            EventBus.Subscribe<CurrencyChangedEvent>(HandleCurrencyChanged);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<ItemPurchasedEvent>(HandlePurchase);
            EventBus.Unsubscribe<CurrencyChangedEvent>(HandleCurrencyChanged);
        }

        public void Refresh()
        {
            if (_coinsText != null)
                _coinsText.text = $"{CurrencyWallet.Instance.Coins}";
        }
    }
}
