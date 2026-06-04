using UnityEngine;

namespace cats
{
    /// <summary>
    /// Coordinates gameplay systems and serves as the entry point
    /// for managing the virtual pet lifecycle.
    /// </summary>
    public class CatController : MonoBehaviour
    {
        [Header("Debug / Test")]
        [SerializeField] private FoodItem _testFoodItem;

        [Tooltip("Duration of a week in seconds for FeedingQualitySystem. 604800 = real week.")]
        [SerializeField] private float _feedingQualityPeriod = 604800f;

        private Cat _cat;
        private FeedAction _feedAction;
        private HealAction _healAction;
        private FeedingQualitySystem _feedingQualitySystem;

        private void Start()
        {
            _cat = new Cat(100f, 100f, 100f);

            // Initializes all gameplay systems responsible for
            // updating the cat's state over time.
            new HungerSystem();
            new MoodSystem();
            new HealthSystem();
            _feedingQualitySystem = new FeedingQualitySystem(_cat, _feedingQualityPeriod);

            _feedAction = new FeedAction();
            _healAction = new HealAction();

            // Forces the UI to display the initial state immediately
            // after application startup.
            EventBus.Publish(new TickEvent { Cat = _cat, DeltaTime = 0f });
        }

        public void OnHeal()
        {
            _healAction.Execute(_cat);
        }

        /// <summary>
        /// Processes feeding requests initiated from the user interface.
        /// </summary>
        public void FeedWithItem(FoodItem food)
        {
            _feedAction.Execute(_cat, food);
        }

        private void Update()
        {
            EventBus.Publish(new TickEvent { Cat = _cat, DeltaTime = Time.deltaTime });

            // Provides a shortcut for validating feeding mechanics
            // during development and balancing.
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_testFoodItem != null)
                {
                    if (!Inventory.Instance.HasFood(_testFoodItem))
                        Inventory.Instance.AddFood(_testFoodItem, 10);

                    _feedAction.Execute(_cat, _testFoodItem);
                }
                else
                {
                    _feedAction.Execute(_cat, _testFoodItem);
                }
            }
        }
    }
}
