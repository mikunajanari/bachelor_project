using UnityEngine;

namespace cats
{
    /// <summary>
    /// Manages satiety changes over time and applies feeding effects,
    /// including food quality and overeating consequences.
    /// </summary>
    public class HungerSystem
    {
        private const float HungerDecayPerPeriod = 3f;

        public HungerSystem()
        {
            EventBus.Subscribe<CatFedEvent>(OnCatFed);
            EventBus.Subscribe<TickEvent>(OnTick);
        }

        private void OnCatFed(CatFedEvent e)
        {
            var cat = e.Cat;
            var food = e.FoodItem;

            // Prevents unlimited feeding when hunger is already full,
            // while still allowing overeating as a gameplay risk.
            if (food != null)
            {
                if (cat.Hunger >= 100f)
                {
                    if (Random.value < cat.OvereatingChance)
                    {
                        cat.AddOvereatingScore(food.OvereatingImpact);
                        ApplyFoodStats(cat, food);
                    }
                    return;
                }

                ApplyFoodStats(cat, food);

                // Records long-term feeding quality for future health evaluation.
                cat.ChangeFeedingQuality(food.Quality.GetFeedingQualityDelta());
            }
            else
            {
                cat.ChangeHunger(e.FoodValue);
            }
        }

        /// <summary>Applies changes to satiety and mood from food.</summary>
        private void ApplyFoodStats(Cat cat, FoodItem food)
        {
            cat.ChangeHunger(food.SatietyGain);
        }

        private void OnTick(TickEvent e)
        {
            // Test mode: satiety decreases by 3 every 30 seconds
            // In release: -3f / 3600f (3 units per hour)
            e.Cat.ChangeHunger(-HungerDecayPerPeriod / 30f * e.DeltaTime);
        }
    }
}
