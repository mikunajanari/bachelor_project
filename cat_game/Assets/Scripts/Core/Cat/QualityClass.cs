namespace cats
{
    /// <summary>
    /// Defines food quality categories used to influence
    /// feeding outcomes and long-term pet development.
    /// </summary>
    public enum QualityClass
    {
        Economy  = 0,
        Premium  = 1,
        Holistic = 2
    }

    public static class QualityClassExtensions
    {
        /// <summary>Quality coefficient for calculating Satiety/Mood changes.</summary>
        public static float GetCoefficient(this QualityClass quality)
        {
            return quality switch
            {
                QualityClass.Economy  => 0.7f,
                QualityClass.Premium  => 1.0f,
                QualityClass.Holistic => 1.2f,
                _                    => 1.0f
            };
        }

        /// <summary>
        /// Tracks the cumulative impact of feeding choices on long-term health.
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
        /// Determines how strongly overeating contributes to weight-related penalties.
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

        /// <summary>Display name for UI.</summary>
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
