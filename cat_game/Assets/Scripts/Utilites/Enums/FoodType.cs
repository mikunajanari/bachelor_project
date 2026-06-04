namespace cats
{
    /// <summary>
    /// Defines food categories used to calculate feeding effects.
    /// Values = base satiation increase (before multiplying by QualityClass).
    /// </summary>
    public enum FoodType
    {
        DryFood  = 20,
        WetFood  = 15,
        Treat    = 5
    }
}
