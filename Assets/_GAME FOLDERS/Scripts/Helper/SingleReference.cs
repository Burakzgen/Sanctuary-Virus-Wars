using UnityEngine;

public class SingleReference<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;
    protected static bool dontDestroyOnLoad = false;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindAnyObjectByType(typeof(T));
                if (instance == null)
                {
                    Debug.LogError("An instance of " + typeof(T) + " is needed in the scene, but there is none.");
                }
            }
            return instance;
        }
    }

    protected virtual void AwakeInternal()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning(gameObject.name + " has been destroyed. Another instance of " + typeof(T) + " already exists.");
            DestroyImmediate(gameObject);
        }
        else
        {
            instance = this as T;
            Initialize();
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }

    protected virtual void Awake()
    {
        AwakeInternal();
    }

    protected virtual void Initialize()
    {
    }

    public static void SetDontDestroyOnLoad(bool value)
    {
        dontDestroyOnLoad = value;
        if (instance != null && dontDestroyOnLoad)
        {
            DontDestroyOnLoad(instance.gameObject);
        }
    }
}