namespace cats
{
    public interface IShopItem
    {
        string Id { get; }
        string DisplayName { get; }
        int CoinPrice { get; }
        decimal MoneyPrice { get; }
        bool CanBuyWithCoins { get; }
        bool CanBuyWithMoney { get; }
    }
}