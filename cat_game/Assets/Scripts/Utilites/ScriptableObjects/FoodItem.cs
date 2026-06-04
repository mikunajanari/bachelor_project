using UnityEngine;

namespace cats
{
    /// <summary>
    /// Defines a purchasable food item and calculates
    /// its gameplay effects based on food type and quality.
    /// </summary>
    [CreateAssetMenu(fileName = "NewFoodItem", menuName = "cats/Items/Food Item")]
    public class FoodItem : ScriptableObject, IShopItem
    {
        [Header("Shop")]
        [SerializeField] private string _id;
        [SerializeField] private string _displayName;
        [SerializeField] private int    _coinPrice;
        [SerializeField] private decimal _moneyPrice;
        [SerializeField] private bool   _canBuyWithCoins  = true;
        [SerializeField] private bool   _canBuyWithMoney  = false;

        public string  Id              => _id;
        public string  DisplayName     => _displayName;
        public int     CoinPrice       => _coinPrice;
        public decimal MoneyPrice      => _moneyPrice;
        public bool    CanBuyWithCoins => _canBuyWithCoins;
        public bool    CanBuyWithMoney => _canBuyWithMoney;

        [Header("Food Properties")]
        [SerializeField] private FoodType    _foodType    = FoodType.DryFood;
        [SerializeField] private QualityClass _quality    = QualityClass.Economy;

        [Header("Display")]
        [SerializeField] private Sprite _icon;
        [SerializeField][TextArea] private string _description;

        public FoodType    FoodType    => _foodType;
        public QualityClass Quality    => _quality;
        public Sprite      Icon        => _icon;
        public string      Description => _description;

        public float SatietyGain =>
            (float)_foodType * _quality.GetCoefficient();

        public float MoodGain
        {
            get
            {
                float baseMood = _foodType switch
                {
                    cats.FoodType.DryFood => 4f,
                    cats.FoodType.WetFood => 6f,
                    cats.FoodType.Treat   => 10f,
                    _                    => 4f
                };
                return baseMood * _quality.GetCoefficient();
            }
        }

        public int OvereatingImpact =>
            _quality.GetOvereatingImpact() + (_foodType == cats.FoodType.Treat ? 2 : 0);
    }
}