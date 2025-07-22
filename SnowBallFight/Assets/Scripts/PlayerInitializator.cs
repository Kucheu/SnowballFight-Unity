using UnityEngine;
using Unity.Netcode;

public class PlayerInitializator : NetworkBehaviour
{
    [SerializeField]
    private NetworkObject playerManagerObject;
    [SerializeField]
    private CameraController cameraController;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }
}
