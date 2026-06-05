using UnityEngine;

namespace cats
{
    /// <summary>
    /// Provides a centralized currency system and persists
    /// the player's coin balance between game sessions.
    /// </summary>
    public class CurrencyWallet
    {
        public static CurrencyWallet Instance { get; } = new CurrencyWallet();

        private const string SaveKey = "PlayerCoins";
        private const int StartingCoins = 100;

        public int Coins { get; private set; }

        private CurrencyWallet()
        {
            Reload();
        }

        public void Reload()
        {
            Coins = PlayerPrefs.GetInt(SaveKey, StartingCoins);
        }

        public bool CanAfford(int amount) => Coins >= amount;

        public bool TrySpend(int amount)
        {
            if (!CanAfford(amount) || amount <= 0) return false;
            Coins -= amount;
            Save();
            EventBus.Publish(new CurrencyChangedEvent { Coins = Coins });

            Debug.Log($"[CurrencyWallet] -{amount} coins. Remaining: {Coins}");
            return true;
        }

        public void Add(int amount)
        {
            if (amount <= 0) return;
            Coins += amount;
            Save();
            EventBus.Publish(new CurrencyChangedEvent { Coins = Coins });
            Debug.Log($"[CurrencyWallet] +{amount} coins. Balance: {Coins}");
        }

        private void Save()
        {
            PlayerPrefs.SetInt(SaveKey, Coins);
            PlayerPrefs.Save();
        }
    }
}
