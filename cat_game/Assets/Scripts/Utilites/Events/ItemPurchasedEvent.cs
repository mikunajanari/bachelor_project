namespace cats
{
    /// <summary>
    /// Подія успішної покупки предмету в магазині.
    /// </summary>
    public struct ItemPurchasedEvent
    {
        public IShopItem Item;
        public bool      PaidWithCoins; // true = монети, false = реальні гроші
    }
}
