using TMPro;
using UnityEngine;

namespace cats
{
    public class CurrencyDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _coinsText;

        private void Start()
        {
            Refresh();
            EventBus.Subscribe<ItemPurchasedEvent>(_ => Refresh());
        }

        public void Refresh()
        {
            if (_coinsText != null)
                _coinsText.text = $"{CurrencyWallet.Instance.Coins}";
        }
    }
}
