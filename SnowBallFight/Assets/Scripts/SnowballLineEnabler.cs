using UnityEngine;

public class SnowballLineEnabler : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private GameObject snowballLineControllerObject;

    private void Update()
    {
        if(playerController.IsOwner)
        {
            snowballLineControllerObject.SetActive(Input.GetMouseButton(1));
        }
    }
}
