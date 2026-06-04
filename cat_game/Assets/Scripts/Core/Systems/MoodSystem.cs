using UnityEngine;

namespace cats
{
    /// <summary>
    /// Manages mood changes based on feeding, healing,
    /// and the overall condition of the cat.
    /// </summary>
    public class MoodSystem
    {
        private const float BaseMoodLoss = 2f;
        private const float CriticalMoodLoss = 4f;
        private const float LowHungerThreshold = 45f;
        private const float LowHealthThreshold = 75f;

        public MoodSystem()
        {
            EventBus.Subscribe<CatHealEvent>(OnCatHeal);
            EventBus.Subscribe<CatFedEvent>(OnCatFed);
            EventBus.Subscribe<TickEvent>(OnTick);
        }

        private void OnCatFed(CatFedEvent e)
        {
            if (e.FoodItem == null) return;
            float moodDelta = Mathf.Round(e.FoodItem.MoodGain);
            e.Cat.ChangeMood(moodDelta);
        }

        private void OnCatHeal(CatHealEvent e)
        {
            e.Cat.ChangeMood(5f);
        }

        private void OnTick(TickEvent e)
        {
            // Test mode: mood decreases by 2 every 30 seconds
            // In release: -2f / 3600f (2 units per hour)
            e.Cat.ChangeMood(-BaseMoodLoss / 30f * e.DeltaTime);

            // Poor physical condition negatively affects emotional well-being.
            if (e.Cat.Hunger < LowHungerThreshold || e.Cat.Health < LowHealthThreshold)
                e.Cat.ChangeMood(-CriticalMoodLoss / 30f * e.DeltaTime);
        }
    }
}
