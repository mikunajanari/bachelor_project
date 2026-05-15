using UnityEngine;

namespace cats
{
    /// <summary>
    /// Система здоров'я.
    /// Враховує: хіл, ожиріння (прискорене зниження здоров'я), тиковий штраф.
    /// FeedingQuality застосовується окремо через PeriodicHealthCheck.
    /// </summary>
    public class HealthSystem
    {
        public HealthSystem()
        {
            EventBus.Subscribe<CatHealEvent>(OnCatHeal);
            EventBus.Subscribe<TickEvent>(OnTick);
        }

        private void OnCatHeal(CatHealEvent e)
        {
            e.Cat.ChangeHealth(e.HealValue);
        }

        private void OnTick(TickEvent e)
        {
            var cat = e.Cat;

            // Базовий штраф при низькій ситості або настрої
            if (cat.Hunger <= 45f || cat.Mood <= 35f)
                cat.ChangeHealth(-4f * e.DeltaTime);

            // При ожирінні здоров'я зменшується швидше
            if (cat.IsObese)
                cat.ChangeHealth(-6f * e.DeltaTime);
            else if (cat.IsOverweight)
                cat.ChangeHealth(-2f * e.DeltaTime);
        }
    }
}
