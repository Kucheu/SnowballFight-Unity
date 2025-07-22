using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.Netcode;
using System;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField]
    private NetworkObject playerControllerPrefab;

    private TeamType currentTeamToSet = TeamType.RedTeam;

    private Dictionary<string, TeamType> playerToTeamType;
    private Dictionary<string, NetworkObject> playerToObject;

    private void OnEnable()
    {
        SnowBall.OnSnowballHit += SnowBall_OnSnowballHit;
    }

    private void OnDisable()
    {
        SnowBall.OnSnowballHit -= SnowBall_OnSnowballHit;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        playerToTeamType = new();
        playerToObject = new();
        OnPlayerLoadRpc(SessionManager.Instance.GetOwnPlayerID());
        //NetworkManager.Singleton.LocalClientId
    }

    [Rpc(SendTo.Server)]
    private void OnPlayerLoadRpc(string playerID, RpcParams rpcParams = default)
    {
        CreateController(playerID, rpcParams.Receive.SenderClientId);
    }

    public void CreateController(string playerId, ulong playerNetworkID)
    {
        TeamType teamType = GetPlayerTeam(playerId);
        Transform spawn = SpawnerManager.Instance.GetRespawnPoint(teamType);
        var newPlayerController = Instantiate(playerControllerPrefab, spawn.position, spawn.rotation, null);
        newPlayerController.SpawnAsPlayerObject(playerNetworkID, destroyWithScene: true);
        newPlayerController.GetComponent<PlayerController>().SetTeam(teamType);
        newPlayerController.GetComponent<PlayerController>().SetOwnerID(playerId);
        if (playerToObject.ContainsKey(playerId))
        {
            playerToObject[playerId] = newPlayerController;
        }
        else
        {
            Debug.LogError(playerId);
            playerToObject.Add(playerId, newPlayerController);
        }
    }

    public TeamType GetPlayerTeam(string playerId)
    {
        TeamType playerTeam;
        if (!playerToTeamType.TryGetValue(playerId, out playerTeam))
        {
            playerToTeamType.Add(playerId, currentTeamToSet);
            playerTeam = currentTeamToSet;
            currentTeamToSet = currentTeamToSet == TeamType.RedTeam ? TeamType.BlueTeam : TeamType.RedTeam;
        }
        return playerTeam;
    }

    private void SnowBall_OnSnowballHit(string killer, string killed)
    {
        OnPlayerDeath(killed);
    }

    private void OnPlayerDeath(string playerId)
    {
        StartCoroutine(SpawnAfterDeathCoroutine(playerId));
    }

    private IEnumerator SpawnAfterDeathCoroutine(string playerId)
    {
        yield return new WaitForSeconds(3f);
        Debug.LogError(playerId);
        if (playerToObject.TryGetValue(playerId, out NetworkObject playerObject))
        {
            ulong playerNetworkID = playerObject.OwnerClientId;
            playerObject.Despawn(true);
            CreateController(playerId, playerNetworkID);
        }
        else
        {
            Debug.LogError("Problem with despawning");
        }
    }
}
