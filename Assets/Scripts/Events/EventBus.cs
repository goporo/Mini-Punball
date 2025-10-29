using System;
using System.Collections.Generic;
using UnityEngine;

// Attribute to mark events that should not be logged
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class DontLogEventAttribute : Attribute { }

public static class EventBus
{
  // Stores all subscribers keyed by event type.
  private static readonly Dictionary<Type, List<Delegate>> _subscribers = new();

  /// <summary>
  /// Subscribe to a specific event type.
  /// </summary>
  public static void Subscribe<T>(Action<T> handler) where T : IGameEvent
  {
    var type = typeof(T);

    if (!_subscribers.TryGetValue(type, out var list))
    {
      list = new List<Delegate>();
      _subscribers[type] = list;
    }

    if (!list.Contains(handler))
      list.Add(handler);
  }

  /// <summary>
  /// Unsubscribe from a specific event type.
  /// </summary>
  public static void Unsubscribe<T>(Action<T> handler) where T : IGameEvent
  {
    var type = typeof(T);

    if (_subscribers.TryGetValue(type, out var list))
    {
      list.Remove(handler);
      if (list.Count == 0)
        _subscribers.Remove(type);
    }
  }

  /// <summary>
  /// Publish (fire) an event to all subscribers.
  /// </summary>
  public static void Publish<T>(T gameEvent) where T : IGameEvent
  {
    var type = typeof(T);

    if (_subscribers.TryGetValue(type, out var list))
    {
      // Copy to avoid modification while iterating
      var tempList = list.ToArray();
      foreach (var del in tempList)
      {
        try
        {
          (del as Action<T>)?.Invoke(gameEvent);
        }
        catch (Exception e)
        {
          Debug.LogError($"[EventBus] Error while invoking {type.Name}: {e}");
        }
      }
    }

#if UNITY_EDITOR
    // Only log if not marked with DontLogEventAttribute
    bool dontLog = Attribute.IsDefined(type, typeof(DontLogEventAttribute));
    if (EventBusSettings.LogEvents && !dontLog)
      Debug.Log($"[EventBus] {type.Name} published");
#endif
  }

  /// <summary>
  /// Clears all subscriptions — useful when changing scenes.
  /// </summary>
  public static void ClearAll()
  {
    _subscribers.Clear();
  }
}


#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Game/EventBus Settings")]
public class EventBusSettings : ScriptableObject
{
  public static bool LogEvents = true;
}
#endif