using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace cats
{
    /// <summary>
    /// UI-елемент одного предмету в магазині.
    /// Прив'яжи у редакторі до відповідних UI-компонентів.
    /// </summary>
    public class ShopItemView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image    _icon;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Button   _buyButton;

        private FoodItem _foodItem;

        public void Setup(FoodItem food)
        {
            _foodItem = food;

            if (_icon != null && food.Icon != null)
                _icon.sprite = food.Icon;

            if (_nameText != null)
                _nameText.text = food.DisplayName;

            if (_descriptionText != null)
                _descriptionText.text =
                    $"{food.FoodType} · {food.Quality.GetDisplayName()}\n{food.Description}";

            if (_priceText != null)
                _priceText.text = food.CanBuyWithCoins ? $"{food.CoinPrice} 🪙" : "—";

            if (_buyButton != null)
            {
                _buyButton.onClick.RemoveAllListeners();
                _buyButton.onClick.AddListener(OnBuyClicked);
            }
        }

        private void OnBuyClicked()
        {
            if (_foodItem == null || PurchaseSystem.Instance == null) return;
            bool success = PurchaseSystem.Instance.BuyFoodWithCoins(_foodItem);
            if (!success)
                Debug.Log("[ShopItemView] Покупка не вдалась (мало монет або недоступно).");
        }
    }
}
