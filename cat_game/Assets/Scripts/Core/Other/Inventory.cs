using System.Collections.Generic;
using UnityEngine;

namespace cats
{
    /// <summary>
    /// Інвентар гравця — зберігає куплені предмети (корм тощо).
    /// Синглтон для простоти MVP.
    /// </summary>
    public class Inventory
    {
        public static Inventory Instance { get; } = new Inventory();

        // FoodItem id → кількість
        private readonly Dictionary<string, int> _foodStock = new();

        private Inventory() { }

        // ── Корм ──────────────────────────────────────────────────

        public void AddFood(FoodItem food, int amount = 1)
        {
            if (!_foodStock.ContainsKey(food.Id))
                _foodStock[food.Id] = 0;
            _foodStock[food.Id] += amount;
            Debug.Log($"[Inventory] +{amount} {food.DisplayName} (total: {_foodStock[food.Id]})");
        }

        public bool HasFood(FoodItem food) =>
            _foodStock.TryGetValue(food.Id, out int qty) && qty > 0;

        /// <summary>Знімає 1 порцію корму. Повертає true якщо успішно.</summary>
        public bool ConsumeFood(FoodItem food)
        {
            if (!HasFood(food)) return false;
            _foodStock[food.Id]--;
            return true;
        }

        public int GetFoodCount(FoodItem food) =>
            _foodStock.TryGetValue(food.Id, out int qty) ? qty : 0;

        public IReadOnlyDictionary<string, int> GetAllFood() => _foodStock;
    }
}
