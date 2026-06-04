using System;
using System.Collections.Generic;

namespace cats
{
    /// <summary>
    /// Provides a lightweight event-driven communication mechanism
    /// that reduces direct dependencies between game systems.
    /// </summary>
    public static class EventBus
    {
        private static readonly Dictionary<Type, Delegate> _events = new();

        public static void Subscribe<T>(Action<T> listener)
        {
            Type key = typeof(T);
            if (_events.TryGetValue(key, out Delegate existing))
                _events[key] = Delegate.Combine(existing, listener);
            else
                _events[key] = listener;
        }

        public static void Unsubscribe<T>(Action<T> listener)
        {
            Type key = typeof(T);
            if (_events.TryGetValue(key, out Delegate existing))
            {
                Delegate result = Delegate.Remove(existing, listener);
                if (result == null)
                    _events.Remove(key);
                else
                    _events[key] = result;
            }
        }

        public static void Publish<T>(T eventData)
        {
            if (_events.TryGetValue(typeof(T), out Delegate action))
                ((Action<T>)action)?.Invoke(eventData);
        }
    }
}
