namespace cats
{
    /// <summary>
    /// Represents a feeding action and transfers all data
    /// required by gameplay systems to process its effects.
    /// </summary>
    public struct CatFedEvent
    {
        public Cat      Cat;
        public float    FoodValue;  // fallback, if FoodItem == null
        public FoodItem FoodItem;   // null = without specific food item
    }
}
