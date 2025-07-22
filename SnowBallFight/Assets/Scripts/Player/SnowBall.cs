using UnityEngine;
using Unity.Netcode;
using System;
using System.Collections;

public class SnowBall : NetworkBehaviour
{
    public delegate void OnSnowballHitDelegate(string killer, string killed);
    public static event OnSnowballHitDelegate OnSnowballHit;

    [SerializeField]
    private Rigidbody snowballRigidbody;
    [SerializeField]
    private Collider snowballCollider;
    [SerializeField]
    private PoolObjectSettings snowballVFXPoolSettings;
    [SerializeField]
    private SnowballSettingsSO snowballSettings;

    private string ownerID;
    private NetworkVariable<TeamType> teamType = new NetworkVariable<TeamType>();

    protected override void OnNetworkPreSpawn(ref NetworkManager networkManager)
    {
        base.OnNetworkPreSpawn(ref networkManager);
        transform.localScale = new Vector3(snowballSettings.size, snowballSettings.size, snowballSettings.size);
        snowballRigidbody.mass = snowballSettings.ballMass;
    }

    protected override void OnNetworkPostSpawn()
    {
        base.OnNetworkPostSpawn();
        if (IsServer)
        {
            StartCoroutine(EnableColliderCoroutine());
        }
    }

    private IEnumerator EnableColliderCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        snowballCollider.enabled = true;
    }

    private void Update()
    {
        if (transform.position.y < -10)
        {
            DestroySnowball();
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        var vfx = PoolFactory.Instance.GetObject(snowballVFXPoolSettings);
        vfx.gameObject.transform.position = transform.position;
        vfx.gameObject.transform.rotation = transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            if (player.GetTeam() != teamType.Value && !player.IsKilled)
            {
                player.GetHitRpc();
                OnSnowballHit?.Invoke(ownerID, player.OwnerID);
            }
        }
        DestroySnowball();
    }

    private void DestroySnowball()
    {
        NetworkObject.Despawn(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 3));
    }

    internal void SetTeam(TeamType value)
    {
        teamType.Value = value;
    }

    public void SetOwner(string ownerID)
    {
        Debug.LogError("Snowball OWNER :" + ownerID);
        this.ownerID = ownerID;
    }

    internal void SetForce()
    {
        snowballRigidbody.AddForce(transform.forward * snowballSettings.throwSpeed, ForceMode.Impulse);
    }
}
