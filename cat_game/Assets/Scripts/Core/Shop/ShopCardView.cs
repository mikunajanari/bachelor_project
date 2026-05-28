using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace cats
{
    public class ShopCardView : MonoBehaviour
    {
        [SerializeField] private FoodItem _foodItem;
        [SerializeField] private TMP_Text _foodName;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private ShopBuyButton[] _buyButtons;

        private void Start()
        {
            if (_foodItem == null) return;
            if (_foodName != null)    _foodName.text = _foodItem.DisplayName;
            if (_description != null) _description.text = $"Опис Товару: {_foodItem.Description}"; // ✅
            
            foreach (var btn in _buyButtons)
                btn.SetFood(_foodItem);
        }
    }
}
