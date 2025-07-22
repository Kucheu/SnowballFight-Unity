using UnityEngine;

public class SnowballLineEnabler : MonoBehaviour
{
    [SerializeField]
    private GameObject snowballLineControllerObject;

    private void Update()
    {
        snowballLineControllerObject.SetActive(Input.GetMouseButton(1));
    }
}
