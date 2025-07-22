using UnityEngine;

public abstract class PoolObject : MonoBehaviour
{
    [HideInInspector]
    public PoolObjectSettings settings;

    public void SetSettings(PoolObjectSettings newSettings)
    {
        settings = newSettings;
        enabled = true;
    }
}
