using UnityEngine;

namespace cats
{
    /// <summary>
    /// Система настрою.
    /// При годуванні використовує MoodGain з FoodItem (тип * клас якості).
    /// </summary>
    public class MoodSystem
    {
        public MoodSystem()
        {
            EventBus.Subscribe<CatHealEvent>(OnCatHeal);
            EventBus.Subscribe<CatFedEvent>(OnCatFed);
            EventBus.Subscribe<TickEvent>(OnTick);
        }

        private void OnCatFed(CatFedEvent e)
        {
            if (e.FoodItem != null)
            {
                // Зміна Настрою: Настрій + Тип корму * Клас якості
                // Округлюємо згідно ТЗ
                float moodDelta = Mathf.Round(e.FoodItem.MoodGain);
                e.Cat.ChangeMood(moodDelta);
            }
            else
            {
                // Fallback
                e.Cat.ChangeMood(5f);
            }
        }

        private void OnCatHeal(CatHealEvent e)
        {
            e.Cat.ChangeMood(5f);
        }

        private void OnTick(TickEvent e)
        {
            // Настрій знижується на 2 за годину → 2/3600 за секунду
            e.Cat.ChangeMood(-2f / 3600f * e.DeltaTime);

            // Додаткове зниження при поганій ситості або здоров'ї
            if (e.Cat.Hunger < 45f || e.Cat.Health <= 75f)
                e.Cat.ChangeMood(-3f * e.DeltaTime);

            // Ожиріння прискорює погіршення здоров'я — обробляється в HealthSystem
        }
    }
}
