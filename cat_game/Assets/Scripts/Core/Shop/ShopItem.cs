using UnityEngine;

namespace cats
{
    [CreateAssetMenu(fileName = "NewShopItem", menuName = "cats/Shop Item")]
    public class ShopItem : ScriptableObject
    {
        [SerializeField] private FoodItem _foodItem;
        [SerializeField] private ShopBuyOption[] _buyOptions;

        public FoodItem FoodItem => _foodItem;
        public ShopBuyOption[] BuyOptions => _buyOptions;
    }

    [System.Serializable]
    public class ShopBuyOption
    {
        [SerializeField] private int _amount;
        [SerializeField] private int _price;

        public int Amount => _amount;
        public int Price => _price;
    }
}
