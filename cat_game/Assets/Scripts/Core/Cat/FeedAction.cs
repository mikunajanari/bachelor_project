using UnityEngine;

namespace cats
{
    /// <summary>
    /// Encapsulates the feeding process and ensures that inventory
    /// consumption and gameplay updates remain synchronized.
    /// </summary>
    public class FeedAction
    {
        /// <summary>
        /// Feeds the cat using an item from the inventory and notifies
        /// all systems that depend on feeding events.
        /// </summary>
        public bool Execute(Cat cat, FoodItem food)
        {
            if (!Inventory.Instance.HasFood(food))
            {
                Debug.Log($"[FeedAction] Food '{food.DisplayName}' is not available in inventory.");
                return false;
            }

            Inventory.Instance.ConsumeFood(food);

            EventBus.Publish(new CatFedEvent
            {
                Cat      = cat,
                FoodItem = food,
                FoodValue = food.SatietyGain
            });

            return true;
        }
    }
}
