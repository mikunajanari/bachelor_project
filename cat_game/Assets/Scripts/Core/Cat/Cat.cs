using UnityEngine;

namespace cats
{
    /// <summary>
    /// Модель кота з усіма показниками згідно ТЗ.
    /// </summary>
    public class Cat
    {
        // ── Основні показники ────────────────────────────────────
        public float Hunger { get; private set; }
        public float Mood   { get; private set; }
        public float Health { get; private set; }

        // ── FeedingQuality ───────────────────────────────────────
        /// <summary>
        /// Накопичений показник якості годування за поточний період.
        /// Перевіряється і обнуляється раз на тиждень (або інший визначений період).
        /// </summary>
        public int FeedingQuality { get; private set; }

        // ── Переїдання ───────────────────────────────────────────
        /// <summary>Ймовірність переїдання при Ситість = 100. Від 0 до 1.</summary>
        public float OvereatingChance { get; private set; }

        /// <summary>Накопичений ефект переїдання.</summary>
        public int OvereatingScore { get; private set; }

        // ── Стани ваги ────────────────────────────────────────────
        public bool IsOverweight => OvereatingScore > 120;
        public bool IsObese      => OvereatingScore > 200;

        // ── Конструктор ──────────────────────────────────────────
        public Cat(float hunger, float mood, float health, float overeatingChance = -1f)
        {
            Hunger = hunger;
            Mood   = mood;
            Health = health;

            // Якщо не передано — рандомізуємо при генерації кота
            OvereatingChance = overeatingChance >= 0f
                ? Mathf.Clamp01(overeatingChance)
                : Random.Range(0f, 1f);
        }

        // ── Основні зміни ────────────────────────────────────────
        public void ChangeHunger(float value)
            => Hunger = Mathf.Clamp(Hunger + value, 0f, 100f);

        public void ChangeMood(float value)
            => Mood = Mathf.Clamp(Mood + value, 0f, 100f);

        public void ChangeHealth(float value)
            => Health = Mathf.Clamp(Health + value, 0f, 100f);

        // ── FeedingQuality ───────────────────────────────────────
        /// <summary>
        /// Викликається при кожному годуванні для зміни FeedingQuality.
        /// delta: -1 (Економ), 0 (Преміум), +1 (Холістик).
        /// </summary>
        public void ChangeFeedingQuality(int delta)
            => FeedingQuality += delta;

        /// <summary>
        /// Застосовує FeedingQuality до Health і обнуляє показник.
        /// Викликається раз на тиждень (або інший ігровий період).
        /// </summary>
        public void ApplyAndResetFeedingQuality()
        {
            int fq = FeedingQuality / 5; // ділимо на 5 згідно ТЗ

            if (FeedingQuality > 0 && FeedingQuality < 5)
                ChangeHealth(1f);
            else if (FeedingQuality < 0 && FeedingQuality > -5)
                ChangeHealth(-1f);
            else
                ChangeHealth(fq);

            FeedingQuality = 0;
        }

        // ── Переїдання ───────────────────────────────────────────
        /// <summary>
        /// Накопичує OvereatingScore після переїдання.
        /// </summary>
        public void AddOvereatingScore(int impact)
            => OvereatingScore = Mathf.Max(0, OvereatingScore + impact);

        /// <summary>
        /// Знижує OvereatingScore за добу без переїдання або від гри.
        /// </summary>
        public void ReduceOvereatingScore(int amount = 1)
            => OvereatingScore = Mathf.Max(0, OvereatingScore - amount);
    }
}
