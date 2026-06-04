using UnityEngine;

namespace cats
{
    public class HealAction
    {
        public void Execute(Cat cat)
        {
            EventBus.Publish(new CatHealEvent
            {
                Cat = cat,
                HealValue = 10
            });
        }
    }
}