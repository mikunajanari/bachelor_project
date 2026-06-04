using TMPro;
using UnityEngine;

namespace cats
{
    public class FoodPriceView : MonoBehaviour
    {
        [SerializeField] private FoodItem _foodItem;
        [SerializeField] private TMP_Text _priceText;

        private void Start()
        {
            if (_foodItem == null || _priceText == null) return;
            _priceText.text = _foodItem.CoinPrice.ToString();
        }
    }
}
