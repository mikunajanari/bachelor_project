using System;
using System.Collections.Generic;

namespace cats
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, Delegate> events = new();

        public static void Subscribe<T>(Action<T> listener)
        {
            if (events.TryGetValue(typeof(T), out var existing))
                events[typeof(T)] = Delegate.Combine(existing, listener);
            else
                events[typeof(T)] = listener;
        }

        public static void Publish<T>(T eventData)
        {
            if (events.TryGetValue(typeof(T), out var action))
                ((Action<T>)action)?.Invoke(eventData);
        }
    }
}