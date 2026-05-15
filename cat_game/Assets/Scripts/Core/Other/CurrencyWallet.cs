using UnityEngine;

namespace cats
{
    /// <summary>
    /// Гаманець гравця (монети).
    /// Синглтон. Синхронізується через PlayerPrefs для збереження між сесіями.
    /// </summary>
    public class CurrencyWallet
    {
        public static CurrencyWallet Instance { get; } = new CurrencyWallet();

        private const string SaveKey = "PlayerCoins";

        public int Coins { get; private set; }

        private CurrencyWallet()
        {
            Coins = PlayerPrefs.GetInt(SaveKey, 100); // стартовий баланс 100 монет
        }

        public bool CanAfford(int amount) => Coins >= amount;

        public bool TrySpend(int amount)
        {
            if (!CanAfford(amount)) return false;
            Coins -= amount;
            Save();
            Debug.Log($"[CurrencyWallet] -{amount} монет. Залишок: {Coins}");
            return true;
        }

        public void Add(int amount)
        {
            Coins += amount;
            Save();
            Debug.Log($"[CurrencyWallet] +{amount} монет. Баланс: {Coins}");
        }

        private void Save() => PlayerPrefs.SetInt(SaveKey, Coins);
    }
}
