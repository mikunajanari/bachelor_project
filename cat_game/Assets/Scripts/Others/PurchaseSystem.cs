using UnityEngine;

namespace cats
{
    public class PurchaseSystem : MonoBehaviour
    {
        public static PurchaseSystem Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        public bool BuyFoodWithCoins(FoodItem food, int quantity = 1)
        {
            if (!food.CanBuyWithCoins)
            {
                Debug.LogWarning($"[PurchaseSystem] {food.DisplayName} не продається за монети.");
                return false;
            }

            int total = food.CoinPrice * quantity;

            if (!CurrencyWallet.Instance.TrySpend(total))
            {
                Debug.Log($"[PurchaseSystem] Недостатньо монет. Потрібно {total}, є {CurrencyWallet.Instance.Coins}.");
                return false;
            }

            Inventory.Instance.AddFood(food, quantity);

            EventBus.Publish(new ItemPurchasedEvent
            {
                Item = food,
                PaidWithCoins = true
            });

            return true;
        }

        public bool BuyItemWithCoins(IShopItem item)
        {
            if (!item.CanBuyWithCoins) return false;
            if (!CurrencyWallet.Instance.TrySpend(item.CoinPrice)) return false;

            if (item is FoodItem food)
                Inventory.Instance.AddFood(food);

            EventBus.Publish(new ItemPurchasedEvent { Item = item, PaidWithCoins = true });
            return true;
        }
    }
}
