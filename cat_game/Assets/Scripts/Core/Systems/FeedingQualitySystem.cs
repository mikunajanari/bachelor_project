using UnityEngine;

namespace cats
{
    /// <summary>
    /// Система тижневої перевірки якості годування.
    /// Раз на <PeriodSeconds> секунд ігрового часу застосовує FeedingQuality до Health
    /// та обнуляє показник.
    /// </summary>
    public class FeedingQualitySystem
    {
        /// <summary>
        /// Тривалість одного «тижня» в ігрових секундах.
        /// За замовчуванням 604800 (реальний тиждень).
        /// Для тестів зменшуй до 60–300.
        /// </summary>
        public float PeriodSeconds { get; set; } = 604800f;

        private float _timer;
        private Cat _cat;

        public FeedingQualitySystem(Cat cat, float periodSeconds = 604800f)
        {
            _cat = cat;
            PeriodSeconds = periodSeconds;
            _timer = 0f;

            EventBus.Subscribe<TickEvent>(OnTick);
        }

        private void OnTick(TickEvent e)
        {
            if (e.Cat != _cat) return; // якщо кілька котів

            _timer += e.DeltaTime;
            if (_timer >= PeriodSeconds)
            {
                _timer -= PeriodSeconds;
                _cat.ApplyAndResetFeedingQuality();
                Debug.Log($"[FeedingQualitySystem] Тижнева перевірка: Health={_cat.Health:F1}");
            }
        }
    }
}
