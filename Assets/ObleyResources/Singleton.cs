using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<T>();

                if (_instance == null)
                {
                    Debug.LogError($"Failed to assign single instence of {nameof(T)}");
                }
            }

            return _instance;
        }
    }

    public static bool TryGetInstance(out T instance)
    {
        if (_instance != null)
        {
            instance = _instance;
            return true;
        }

        instance = FindFirstObjectByType<T>();

        if (instance == null)
        {
            return false;
        }

        _instance = instance;

        return true;
    }

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning($"Duplicate singleton {typeof(T)} found. Destroying new instance.");
            Destroy(gameObject);
            return;
        }

        _instance = (T)this;
    }
}
