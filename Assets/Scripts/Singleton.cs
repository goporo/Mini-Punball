using UnityEngine;

/// <summary>
/// Base class for inheritable MonoBehaviour singletons.
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
  private static T _instance;
  private static bool _shuttingDown;
  private static readonly object _lock = new();

  public static T Instance
  {
    get
    {
      if (_shuttingDown) return null;

      lock (_lock)
      {
        if (_instance == null)
        {
          // Find an existing instance.
          _instance = (T)FindObjectOfType(typeof(T));

          // If none, create a new GameObject.
          if (_instance == null)
          {
            var singletonObject = new GameObject($"{typeof(T)} (Singleton)");
            _instance = singletonObject.AddComponent<T>();
          }
        }
        return _instance;
      }
    }
  }

  protected virtual void Awake()
  {
    // Prevent duplicates.
    if (_instance == null)
    {
      _instance = this as T;
    }
    else if (_instance != this)
    {
      Destroy(gameObject);
    }
  }

  protected virtual void OnApplicationQuit() => _shuttingDown = true;
  protected virtual void OnDestroy() => _shuttingDown = true;
}
