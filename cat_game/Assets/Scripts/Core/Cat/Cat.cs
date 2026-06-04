using UnityEngine;

namespace cats
{
    /// <summary>
    /// Represents the cat state and stores all gameplay-related attributes.
    /// </summary>
    public class Cat
    {
        // Core attributes that define the current condition of the cat.
        public float Hunger { get; private set; }
        public float Mood   { get; private set; }
        public float Health { get; private set; }

        // Tracks the long-term impact of feeding choices on health.
        public int FeedingQuality { get; private set; }

        /// <summary>
        /// Defines how likely the cat is to eat when already fully satiated.
        /// </summary>
        public float OvereatingChance { get; private set; }

        /// <summary>
        /// Tracks the cumulative effect of overeating.
        /// </summary>
        public int OvereatingScore { get; private set; }

        // Weight states are derived from the accumulated overeating effect.
        public bool IsOverweight => OvereatingScore > 120;
        public bool IsObese      => OvereatingScore > 200;


        public Cat(float hunger, float mood, float health, float overeatingChance = -1f)
        {
            Hunger = hunger;
            Mood   = mood;
            Health = health;

            // Allows each generated cat to have a unique tendency toward overeating.
            OvereatingChance = overeatingChance >= 0f
                ? Mathf.Clamp01(overeatingChance)
                : Random.Range(0f, 1f);
        }

        // Ensures attribute values remain within valid gameplay boundaries.
        public void ChangeHunger(float value)
            => Hunger = Mathf.Clamp(Hunger + value, 0f, 100f);

        public void ChangeMood(float value)
            => Mood = Mathf.Clamp(Mood + value, 0f, 100f);

        public void ChangeHealth(float value)
            => Health = Mathf.Clamp(Health + value, 0f, 100f);

        /// <summary>
        /// Called when the cat is fed to change the FeedingQuality.
        /// delta: -1 (Economic), 0 (Premium), +1 (Holistically).
        /// </summary>
        public void ChangeFeedingQuality(int delta)
            => FeedingQuality += delta;

        /// <summary>
        /// Applies the FeedingQuality to Health and resets the indicator.
        /// Called once per period.
        /// </summary>
        public void ApplyAndResetFeedingQuality()
        {
            int fq = FeedingQuality / 5;

            if (FeedingQuality > 0 && FeedingQuality < 5)
                ChangeHealth(1f);
            else if (FeedingQuality < 0 && FeedingQuality > -5)
                ChangeHealth(-1f);
            else
                ChangeHealth(fq);

            FeedingQuality = 0;
        }

        /// <summary>
        /// Accumulates OvereatingScore after overeating.
        /// </summary>
        public void AddOvereatingScore(int impact)
            => OvereatingScore = Mathf.Max(0, OvereatingScore + impact);

        /// <summary>
        /// Rewards healthy behavior by gradually reducing accumulated overeating effects.
        /// </summary>
        public void ReduceOvereatingScore(int amount = 1)
            => OvereatingScore = Mathf.Max(0, OvereatingScore - amount);
    }
}
