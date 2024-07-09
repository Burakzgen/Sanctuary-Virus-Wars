using UnityEngine;

public class SingleReference<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;

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

    public virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning(gameObject.name + " has been destroyed. Another instance of " + typeof(T) + " already exists.");
            DestroyImmediate(gameObject);
        }
        else
        {
            Initialize();
        }
    }

    protected virtual void Initialize()
    {
    }
}
