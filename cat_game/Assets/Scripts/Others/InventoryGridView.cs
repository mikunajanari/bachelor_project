using System.Collections.Generic;
using UnityEngine;

namespace cats
{
    public class InventoryGridView : MonoBehaviour
    {
        [SerializeField] private DraggableFoodSlot _slotPrefab;
        [SerializeField] private Transform _gridContainer;
        [SerializeField] private int _totalSlots = 6;
        [SerializeField] private List<FoodItem> _allFoodItems;

        private readonly List<DraggableFoodSlot> _slots = new();

        private void Start()
        {
            BuildGrid();
            Refresh();

            EventBus.Subscribe<ItemPurchasedEvent>(OnInventoryChanged);
            EventBus.Subscribe<CatFedEvent>(OnInventoryChanged);
        }

        private void OnInventoryChanged<T>(T _) => Refresh();

        private void BuildGrid()
        {
            foreach (Transform child in _gridContainer)
                Destroy(child.gameObject);
            _slots.Clear();

            for (int i = 0; i < _totalSlots; i++)
            {
                DraggableFoodSlot slot = Instantiate(_slotPrefab, _gridContainer);
                slot.SetEmpty();
                _slots.Add(slot);
            }
        }

        private void Refresh()
        {
            var stock = Inventory.Instance.GetAllFood();
            var items = new List<(FoodItem food, int count)>();

            foreach (var kvp in stock)
            {
                if (kvp.Value <= 0) continue;
                FoodItem food = FindFood(kvp.Key);
                if (food != null) items.Add((food, kvp.Value));
            }

            for (int i = 0; i < _slots.Count; i++)
            {
                if (i < items.Count)
                    _slots[i].Setup(items[i].food, items[i].count);
                else
                    _slots[i].SetEmpty();
            }
        }

        private FoodItem FindFood(string key)
        {
            foreach (var food in _allFoodItems)
            {
                if (food == null) continue;
                string foodKey = string.IsNullOrEmpty(food.Id) ? food.name : food.Id;
                if (foodKey == key) return food;
            }
            return null;
        }
    }
}
