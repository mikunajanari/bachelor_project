using UnityEngine;

namespace cats
{
    /// <summary>
    /// Дія годування кота конкретним кормом з інвентаря.
    /// Якщо корм є в інвентарі — витрачає порцію і публікує CatFedEvent.
    /// </summary>
    public class FeedAction
    {
        /// <summary>Погодувати кота конкретним кормом з інвентаря.</summary>
        public bool Execute(Cat cat, FoodItem food)
        {
            if (!Inventory.Instance.HasFood(food))
            {
                Debug.Log($"[FeedAction] Нема корму '{food.DisplayName}' в інвентарі.");
                return false;
            }

            Inventory.Instance.ConsumeFood(food);

            EventBus.Publish(new CatFedEvent
            {
                Cat      = cat,
                FoodItem = food,
                FoodValue = food.SatietyGain // для сумісності з legacy-підписниками
            });

            return true;
        }

        /// <summary>Legacy: годування без конкретного корму (для тестів / клавіша F).</summary>
        public void Execute(Cat cat)
        {
            EventBus.Publish(new CatFedEvent
            {
                Cat       = cat,
                FoodItem  = null,
                FoodValue = 20f
            });
        }
    }
}
