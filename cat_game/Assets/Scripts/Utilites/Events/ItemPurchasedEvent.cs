namespace cats
{
    public struct ItemPurchasedEvent
    {
        public IShopItem Item;
        public bool PaidWithCoins;
        public FoodItem FoodItem;
        public int Amount;
    }
}
