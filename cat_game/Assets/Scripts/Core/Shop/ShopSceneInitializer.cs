using System.Collections.Generic;
using UnityEngine;

namespace cats
{
    /// <summary>
    /// Ініціалізує магазин: спавнить ShopItemView для кожного FoodItem зі списку.
    /// Прив'яжи у редакторі масив доступних FoodItem та prefab картки.
    /// </summary>
    public class ShopSceneInitializer : MonoBehaviour
    {
        [Header("Catalog")]
        [SerializeField] private List<FoodItem> _availableFoods;

        [Header("Prefab & Container")]
        [SerializeField] private GameObject _shopItemViewPrefab;
        [SerializeField] private Transform  _itemContainer;

        private void Start()
        {
            if (_availableFoods == null || _shopItemViewPrefab == null || _itemContainer == null)
            {
                Debug.LogError("[ShopSceneInitializer] Перевір прив'язки у редакторі!");
                return;
            }

            foreach (var food in _availableFoods)
            {
                var go = Instantiate(_shopItemViewPrefab, _itemContainer);
                var view = go.GetComponent<ShopItemView>();
                if (view != null) view.Setup(food);
            }
        }
    }
}
