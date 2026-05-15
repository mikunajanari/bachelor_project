using UnityEngine;

namespace cats
{
    /// <summary>
    /// ScriptableObject для опису одиниці корму.
    /// Реалізує IShopItem — може продаватись у магазині за монети або реальні гроші.
    /// </summary>
    [CreateAssetMenu(fileName = "NewFoodItem", menuName = "CoonGame/Items/Food Item")]
    public class FoodItem : ScriptableObject, IShopItem
    {
        // ─── IShopItem ───────────────────────────────────────────
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

        // ─── Food properties ─────────────────────────────────────
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

        // ─── Computed helpers ────────────────────────────────────

        /// <summary>
        /// Базовий приріст ситості = (int)FoodType * коефіцієнт якості.
        /// Наприклад, DryFood Holistic: 20 * 1.2 = 24
        /// </summary>
        public float SatietyGain =>
            (float)_foodType * _quality.GetCoefficient();

        /// <summary>
        /// Приріст настрою згідно ТЗ:
        /// Тип корму "смаколик" дає менший приріст ситості але додаткові бонуси.
        /// За замовчуванням = (treat ? 4 : (int)FoodType * 0.2f) * coefficient
        /// Для простоти: MoodGain = FoodType == Treat ? 4 : (int)FoodType * 0.2f
        /// помножений на coefficient.
        /// </summary>
        public float MoodGain
        {
            get
            {
                float baseMood = _foodType == cats.FoodType.Treat
                    ? 4f
                    : (float)_foodType * 0.2f;
                return baseMood * _quality.GetCoefficient();
            }
        }

        /// <summary>
        /// Вплив на OvereatingScore при переїданні.
        /// Смаколик додає +2 до базового значення класу якості.
        /// </summary>
        public int OvereatingImpact =>
            _quality.GetOvereatingImpact() + (_foodType == cats.FoodType.Treat ? 2 : 0);
    }
}
