using UnityEngine;

public class CameraController : MonoBehaviourSingleton<CameraController>
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector3 offset;

    private Transform target;

    private Vector3 cameraTargetPosition
    {
        get
        {
            return target.position + (target.forward * offset.z) + (target.right * offset.x) + (target.up * offset.y);
        }
    }

    public void FollowObject(Transform target)
    {
        this.target = target;
    }

    private void Update()
    {
        if (target == null) return;

        Vector3 direction = cameraTargetPosition - transform.position;
        float distance = Vector3.Distance(transform.position, cameraTargetPosition);
        transform.position += direction.normalized * (distance > speed ? speed : distance);

        transform.rotation = target.rotation;
    }
}
