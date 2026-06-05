using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace cats
{
    /// <summary>
    /// Centralized storage of all food items owned by the player.
    /// </summary>
    public class Inventory
    {
        public static Inventory Instance { get; } = new Inventory();

        private const string FileName = "inventory.json";
        private readonly Dictionary<string, int> _foodStock = new();

        private Inventory()
        {
            Load();
        }

        public void AddFood(FoodItem food, int amount = 1)
        {
            if (food == null || amount <= 0) return;

            string key = GetKey(food);
            if (!_foodStock.ContainsKey(key))
                _foodStock[key] = 0;
            _foodStock[key] += amount;
            Save();
            Debug.Log($"[Inventory] +{amount} {food.DisplayName} (In all: {_foodStock[key]})");
        }

        public bool HasFood(FoodItem food) =>
            _foodStock.TryGetValue(GetKey(food), out int qty) && qty > 0;

        public bool ConsumeFood(FoodItem food)
        {
            if (!HasFood(food)) return false;

            string key = GetKey(food);
            _foodStock[key]--;
            if (_foodStock[key] <= 0)
                _foodStock.Remove(key);

            Save();
            return true;
        }

        public int GetFoodCount(FoodItem food) =>
            _foodStock.TryGetValue(GetKey(food), out int qty) ? qty : 0;

        public IReadOnlyDictionary<string, int> GetAllFood() => _foodStock;

        private void Save()
        {
            try
            {
                var data = new InventorySaveData();

                foreach (var item in _foodStock)
                {
                    if (item.Value <= 0) continue;
                    data.Items.Add(new InventoryItemData { Id = item.Key, Count = item.Value });
                }

                string path = Path.Combine(Application.persistentDataPath, FileName);
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(path, json);

                Debug.Log($"[Inventory] Saved to {path}");
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[Inventory] Failed to save inventory: {ex.Message}");
            }
        }

        private void Load()
        {
            try
            {
                _foodStock.Clear();

                string path = Path.Combine(Application.persistentDataPath, FileName);
                if (!File.Exists(path)) return;

                string json = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(json)) return;

                var data = JsonUtility.FromJson<InventorySaveData>(json);
                if (data?.Items == null) return;

                foreach (var item in data.Items)
                {
                    if (item == null || item.Count <= 0 || string.IsNullOrWhiteSpace(item.Id))
                        continue;

                    _foodStock[item.Id] = item.Count;
                }

                Debug.Log($"[Inventory] Loaded from {path}");
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[Inventory] Failed to load saved inventory: {ex.Message}");
            }
        }

        private string GetKey(FoodItem food) =>
            string.IsNullOrEmpty(food.Id) ? food.name : food.Id;

        [Serializable]
        private class InventorySaveData
        {
            public List<InventoryItemData> Items = new();
        }

        [Serializable]
        private class InventoryItemData
        {
            public string Id;
            public int Count;
        }
    }
}
