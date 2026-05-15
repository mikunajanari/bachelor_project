using UnityEngine;

namespace cats
{
    /// <summary>
    /// MonoBehaviour-обгортка навколо ShopItemData або FoodItem.
    /// Використовується для прив'язки предметів до об'єктів у сцені.
    /// </summary>
    public class GameItem : MonoBehaviour
    {
        [SerializeField] private ScriptableObject _itemData;

        public IShopItem Item => _itemData as IShopItem;
        public FoodItem  FoodItem => _itemData as FoodItem;
    }
}
