using UnityEngine;

public abstract class SingletonPersist<T> : MonoBehaviour where T : MonoBehaviour
{
  private static T _instance;
  private static readonly object _lock = new object();
  private static bool _quitting = false;

  public static T Instance
  {
    get
    {
      if (_quitting) return null;

      lock (_lock)
      {
        if (_instance == null)
        {
          _instance = FindObjectOfType<T>();

          if (_instance == null)
          {
            // Optional: auto-create an object if missing
            var singletonObject = new GameObject(typeof(T).Name);
            _instance = singletonObject.AddComponent<T>();
            DontDestroyOnLoad(singletonObject);
          }
        }

        return _instance;
      }
    }
  }

  protected virtual void Awake()
  {
    if (_instance == null)
    {
      _instance = this as T;
      DontDestroyOnLoad(gameObject);
    }
    else if (_instance != this)
    {
      Destroy(gameObject); // Prevent duplicates
    }
  }

  protected virtual void OnApplicationQuit()
  {
    _quitting = true;
  }
}
