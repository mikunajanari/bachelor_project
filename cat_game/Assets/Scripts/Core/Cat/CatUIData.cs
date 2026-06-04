using UnityEngine;

namespace cats.UI
{
    public class CatUIData
    {
        public float Hunger { get; private set; }
        public float Mood { get; private set; }
        public float Health { get; private set; }

        public void UpdateStats(Cat cat)
        {
            Hunger = cat.Hunger;
            Mood = cat.Mood;
            Health = cat.Health;
        }
    }
}