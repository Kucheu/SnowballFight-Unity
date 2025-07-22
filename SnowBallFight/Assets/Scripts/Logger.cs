using UnityEngine;
using System.Text;

public class Logger 
{
    public static void LogMissingComponent(string componentName, GameObject target = null)
    {
        Debug.LogError($"Missing Component <color=#2a9df4>{componentName}</color>", target);
    }
}
