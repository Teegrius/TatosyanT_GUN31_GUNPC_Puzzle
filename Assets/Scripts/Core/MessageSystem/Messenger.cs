using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.MessageSystem
{
    public static class Messenger
    {
        private static readonly Dictionary<Type, HashSet<IMessageListener>> _listeners = new();

        public static void Clear() => _listeners.Clear();

        public static void Send<TMessage>(TMessage message)
        {
            if (_listeners.TryGetValue(typeof(TMessage), out var listeners))
            {
                foreach (var listener in listeners)
                {
                    ((IMessageListener<TMessage>)listener).OnMessage(message);
                }
            }
            else
            {
                Debug.LogError($"No listener is registered to {typeof(TMessage).Name}");
            }
        }

        public static void Subscribe<TMessage>(IMessageListener<TMessage> messageListener)
        {
            if (_listeners.TryGetValue(typeof(TMessage), out var receivers))
            {
                if (receivers.Contains(messageListener))
                {
                    Debug.LogError($"{messageListener.GetType().Name} already subscribed to {typeof(TMessage).Name}");
                    return;
                }
                receivers.Add(messageListener);
            }
            else
            {
                _listeners.Add(typeof(TMessage), new HashSet<IMessageListener> {messageListener});
            }
        }

        public static void Unsubscribe<TMessage>(IMessageListener<TMessage> messageListener)
        {
            if (_listeners.TryGetValue(typeof(TMessage), out var receivers))
            {
                receivers.Remove(messageListener);
            }
            else
            {
                Debug.LogError($"{messageListener.GetType().Name} is not registered for {typeof(TMessage).Name}");
            }
        }
    }
}