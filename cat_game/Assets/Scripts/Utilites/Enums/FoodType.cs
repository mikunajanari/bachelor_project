namespace cats
{
    /// <summary>
    /// Тип корму. Визначає базовий вплив на Ситість і Настрій при годуванні.
    /// Значення = базовий приріст ситості (до множення на QualityClass).
    /// </summary>
    public enum FoodType
    {
        DryFood  = 20,   // Сухий корм
        WetFood  = 30,   // Вологий корм
        Treat    = 10    // Смаколик (+2 до OvereatingImpact)
    }
}
