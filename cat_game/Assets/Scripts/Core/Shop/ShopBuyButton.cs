using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace cats
{
    public class ShopBuyButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _amountText;
        [SerializeField] private int _amount = 1;

        private FoodItem _foodItem;

        private void Awake()
        {
            _button.onClick.AddListener(OnClick);
            _button.interactable = false;
        }

        public void SetFood(FoodItem foodItem)
        {
            _foodItem = foodItem;

            if (_amountText != null) _amountText.text = $"x{_amount}";
            if (_priceText != null) _priceText.text = (_foodItem.CoinPrice * _amount).ToString();

            Refresh();
        }

        public void Refresh()
        {
            if (_foodItem == null) return;
            _button.interactable = CurrencyWallet.Instance.CanAfford(_foodItem.CoinPrice * _amount);
        }

        private void OnClick()
        {
            if (_foodItem == null) return;
            PurchaseSystem.Instance.BuyFoodWithCoins(_foodItem, _amount);
            Refresh();
        }
    }
}
