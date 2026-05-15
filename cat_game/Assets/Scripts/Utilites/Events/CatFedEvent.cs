namespace cats
{
    /// <summary>
    /// Подія годування кота.
    /// FoodItem містить всю інформацію про корм (тип, клас якості, приріст ситості/настрою).
    /// Якщо годуємо без конкретного Item (наприклад, тест), FoodItem може бути null — 
    /// тоді береться FoodValue як є.
    /// </summary>
    public struct CatFedEvent
    {
        public Cat      Cat;
        public float    FoodValue;  // legacy / fallback, якщо FoodItem == null
        public FoodItem FoodItem;   // null = без конкретного корму
    }
}
