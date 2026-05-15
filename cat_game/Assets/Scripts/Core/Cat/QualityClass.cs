namespace cats
{
    /// <summary>
    /// Клас якості корму.
    /// Coefficient — множник для розрахунку зміни Ситості / Настрою.
    /// FeedingQualityDelta — вплив на FeedingQuality при кожному годуванні.
    /// OvereatingImpact — базовий вплив на OvereatingScore при переїданні.
    /// </summary>
    public enum QualityClass
    {
        Economy  = 0,   // Економ
        Premium  = 1,   // Преміум
        Holistic = 2    // Холістик
    }

    public static class QualityClassExtensions
    {
        /// <summary>Коефіцієнт якості для розрахунку Ситості/Настрою.</summary>
        public static float GetCoefficient(this QualityClass quality)
        {
            return quality switch
            {
                QualityClass.Economy  => 1.0f,
                QualityClass.Premium  => 1.1f,
                QualityClass.Holistic => 1.2f,
                _                    => 1.0f
            };
        }

        /// <summary>
        /// Зміна FeedingQuality при кожному годуванні:
        ///   Економ  = -1, Преміум = 0, Холістик = +1
        /// </summary>
        public static int GetFeedingQualityDelta(this QualityClass quality)
        {
            return quality switch
            {
                QualityClass.Economy  => -1,
                QualityClass.Premium  =>  0,
                QualityClass.Holistic =>  1,
                _                    =>  0
            };
        }

        /// <summary>
        /// Базовий вплив на OvereatingScore при переїданні:
        ///   Економ = +3, Преміум = +2, Холістик = +1
        /// </summary>
        public static int GetOvereatingImpact(this QualityClass quality)
        {
            return quality switch
            {
                QualityClass.Economy  => 3,
                QualityClass.Premium  => 2,
                QualityClass.Holistic => 1,
                _                    => 1
            };
        }

        /// <summary>Назва для UI.</summary>
        public static string GetDisplayName(this QualityClass quality)
        {
            return quality switch
            {
                QualityClass.Economy  => "Економ",
                QualityClass.Premium  => "Преміум",
                QualityClass.Holistic => "Холістик",
                _                    => "—"
            };
        }
    }
}
