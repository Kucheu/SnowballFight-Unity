using UnityEngine;
using Unity.Netcode.Components;
using Unity.Netcode;
using System.Collections.Generic;

public class SpawnerManager : NetworkBehaviourSingleton<SpawnerManager>
{
    [SerializeField]
    private List<Transform> blueSpawns;
    [SerializeField]
    private List<Transform> redSpawns;

    public Transform GetRespawnPoint(TeamType team)
    {
        return team == TeamType.RedTeam ? redSpawns[UnityEngine.Random.Range(0, redSpawns.Count)] : blueSpawns[UnityEngine.Random.Range(0, blueSpawns.Count)];
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }
}
