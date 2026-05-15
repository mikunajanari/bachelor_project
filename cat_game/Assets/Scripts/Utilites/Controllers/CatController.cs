using UnityEngine;

namespace cats
{
    /// <summary>
    /// Контролер кота. Ініціалізує всі системи та прив'язує дії.
    /// </summary>
    public class CatController : MonoBehaviour
    {
        [Header("Debug / Test")]
        [Tooltip("FoodItem для тесту (клавіша F). Якщо null — legacy режим без Item.")]
        [SerializeField] private FoodItem _testFoodItem;

        [Tooltip("Тривалість тижня в секундах для FeedingQualitySystem. 604800 = реальний тиждень.")]
        [SerializeField] private float _feedingQualityPeriod = 604800f;

        private Cat _cat;
        private FeedAction _feedAction;
        private HealAction _healAction;
        private FeedingQualitySystem _feedingQualitySystem;

        private void Start()
        {
            _cat = new Cat(100f, 100f, 100f);

            // Ініціалізуємо системи (підписуються на EventBus у конструкторі)
            new HungerSystem();
            new MoodSystem();
            new HealthSystem();
            _feedingQualitySystem = new FeedingQualitySystem(_cat, _feedingQualityPeriod);

            _feedAction = new FeedAction();
            _healAction = new HealAction();

            // Перший тік для ініціалізації UI
            EventBus.Publish(new TickEvent { Cat = _cat, DeltaTime = 0f });
        }

        public void OnHeal()
        {
            _healAction.Execute(_cat);
        }

        /// <summary>Годування конкретним кормом з інвентаря (виклик з UI).</summary>
        public void FeedWithItem(FoodItem food)
        {
            _feedAction.Execute(_cat, food);
        }

        private void Update()
        {
            EventBus.Publish(new TickEvent { Cat = _cat, DeltaTime = Time.deltaTime });

            // Тест: F — годування тестовим кормом або legacy
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_testFoodItem != null)
                {
                    // Для тесту додаємо корм в інвентар якщо його нема
                    if (!Inventory.Instance.HasFood(_testFoodItem))
                        Inventory.Instance.AddFood(_testFoodItem, 10);

                    _feedAction.Execute(_cat, _testFoodItem);
                }
                else
                {
                    _feedAction.Execute(_cat);
                }
            }
        }
    }
}
