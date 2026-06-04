using UnityEngine;

namespace cats
{
    /// <summary>
    /// Applies long-term health effects based on accumulated
    /// feeding quality over a configurable evaluation period.
    /// </summary>
    public class FeedingQualitySystem
    {
        /// Defaults to 604800 (one real week).
        /// For testing, reduce to 60–300.
        private const float WeekInSeconds = 604800f;

        public float PeriodSeconds { get; set; } = WeekInSeconds;

        private float _timer;
        private Cat _cat;

        public FeedingQualitySystem(Cat cat, float periodSeconds = WeekInSeconds)
        {
            _cat = cat;
            PeriodSeconds = periodSeconds;
            _timer = 0f;

            EventBus.Subscribe<TickEvent>(OnTick);
        }

        private void OnTick(TickEvent e)
        {
            if (e.Cat != _cat) return;

            _timer += e.DeltaTime;
            if (_timer >= PeriodSeconds)
            {
                _timer -= PeriodSeconds;
                _cat.ApplyAndResetFeedingQuality();
                Debug.Log($"[FeedingQualitySystem] Weekly check: Health={_cat.Health:F1}");
            }
        }
    }
}
