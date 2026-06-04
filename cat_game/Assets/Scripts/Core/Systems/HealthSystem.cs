using UnityEngine;

namespace cats
{
    /// <summary>
    /// Manages health changes caused by healing actions,
    /// poor pet care, and weight-related conditions.
    /// </summary>
    public class HealthSystem
    {
        // Base penalty for low satiety or mood
        // Test mode: health decreases by 1 every 30 seconds
        // In release: -1f / 3600f (-1 per hour)
        private const float HealthPenalty = 1f / 30f;
        private const float OverweightPenalty = 1f / 30f;
        // Test mode: health decreases by 2 every 30 seconds
        // In release: -2f / 3600f (-2 per hour)
        private const float ObesityPenalty = 2f / 30f;

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

            if (cat.Hunger <= 45f || cat.Mood <= 35f)
                cat.ChangeHealth(-HealthPenalty * e.DeltaTime);

            
            if (cat.IsObese)
                cat.ChangeHealth(-ObesityPenalty * e.DeltaTime);
            else if (cat.IsOverweight)
                cat.ChangeHealth(-OverweightPenalty * e.DeltaTime);
        }
    }
}
