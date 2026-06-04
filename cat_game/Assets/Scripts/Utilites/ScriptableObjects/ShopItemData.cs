using UnityEngine;

namespace cats
{
    /// <summary>
    /// Represents a generic shop item that can be purchased
    /// and displayed in the in-game store.
    /// </summary>
    [CreateAssetMenu(fileName = "NewShopItem", menuName = "cats/Items/Shop Item")]
    public class ShopItemData : ScriptableObject, IShopItem
    {
        [Header("Shop")]
        [SerializeField] private string  _id;
        [SerializeField] private string  _displayName;
        [SerializeField] private int     _coinPrice;
        [SerializeField] private decimal _moneyPrice;
        [SerializeField] private bool    _canBuyWithCoins = true;
        [SerializeField] private bool    _canBuyWithMoney = false;

        [Header("Display")]
        [SerializeField] private Sprite  _icon;
        [SerializeField][TextArea] private string _description;

        public string  Id              => _id;
        public string  DisplayName     => _displayName;
        public int     CoinPrice       => _coinPrice;
        public decimal MoneyPrice      => _moneyPrice;
        public bool    CanBuyWithCoins => _canBuyWithCoins;
        public bool    CanBuyWithMoney => _canBuyWithMoney;
        public Sprite  Icon            => _icon;
        public string  Description     => _description;
    }
}
