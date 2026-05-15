using UnityEngine;

namespace cats
{
    /// <summary>
    /// Система ситості.
    /// Обробляє годування з урахуванням FoodItem (тип + клас якості) згідно ТЗ.
    /// Також обробляє логіку переїдання через OvereatingChance.
    /// </summary>
    public class HungerSystem
    {
        public HungerSystem()
        {
            EventBus.Subscribe<CatFedEvent>(OnCatFed);
            EventBus.Subscribe<TickEvent>(OnTick);
        }

        private void OnCatFed(CatFedEvent e)
        {
            var cat = e.Cat;
            var food = e.FoodItem;

            // ── Якщо корм конкретний (через FoodItem) ─────────────
            if (food != null)
            {
                // Якщо кіт вже ситий — перевіряємо OvereatingChance
                if (cat.Hunger >= 100f)
                {
                    if (Random.value < cat.OvereatingChance)
                    {
                        // Кіт переїдає
                        cat.AddOvereatingScore(food.OvereatingImpact);
                        ApplyFoodStats(cat, food);
                    }
                    // Інакше — кіт відмовляється від корму (RefuseFood)
                    return;
                }

                ApplyFoodStats(cat, food);

                // Записуємо FeedingQuality
                cat.ChangeFeedingQuality(food.Quality.GetFeedingQualityDelta());
            }
            else
            {
                // ── Fallback: без FoodItem, просто FoodValue ───────
                cat.ChangeHunger(e.FoodValue);
            }
        }

        /// <summary>Застосовує зміни Ситості та Настрою від корму.</summary>
        private void ApplyFoodStats(Cat cat, FoodItem food)
        {
            cat.ChangeHunger(food.SatietyGain);
            // Зміна настрою публікується окремо через MoodSystem,
            // але ми кладемо значення у CatFedEvent через MoodGain.
            // Щоб уникнути дублювання — MoodSystem сам читає FoodItem.
        }

        private void OnTick(TickEvent e)
        {
            // Ситість знижується на 3 за годину → 3/3600 за секунду
            e.Cat.ChangeHunger(-3f / 3f * e.DeltaTime);
        }
    }
}
