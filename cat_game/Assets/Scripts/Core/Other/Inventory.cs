using System.Collections.Generic;
using UnityEngine;

namespace cats
{
    public class Inventory
    {
        public static Inventory Instance { get; } = new Inventory();

        private readonly Dictionary<string, int> _foodStock = new();

        private Inventory() { }

        public void AddFood(FoodItem food, int amount = 1)
        {
            string key = GetKey(food);
            if (!_foodStock.ContainsKey(key))
                _foodStock[key] = 0;
            _foodStock[key] += amount;
            Debug.Log($"[Inventory] +{amount} {food.DisplayName} (всього: {_foodStock[key]})");
        }

        public bool HasFood(FoodItem food) =>
            _foodStock.TryGetValue(GetKey(food), out int qty) && qty > 0;

        public bool ConsumeFood(FoodItem food)
        {
            if (!HasFood(food)) return false;
            _foodStock[GetKey(food)]--;
            return true;
        }

        public int GetFoodCount(FoodItem food) =>
            _foodStock.TryGetValue(GetKey(food), out int qty) ? qty : 0;

        public IReadOnlyDictionary<string, int> GetAllFood() => _foodStock;

        private string GetKey(FoodItem food) =>
            string.IsNullOrEmpty(food.Id) ? food.name : food.Id;
    }
}
