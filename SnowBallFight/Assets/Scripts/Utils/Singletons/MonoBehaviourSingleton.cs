using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance
    {
        get => instance;
        set
        {
            if(instance != null)
            {
                Debug.LogError("Instance is already set");
                Destroy(value as MonoBehaviour);
            }
            instance = value;
        }
    }

    private static T instance;

    protected virtual void Awake()
    {
        Instance = this as T;
    }

}