using UnityEngine;
using Unity.Netcode;
using System;
using System.Collections.Generic;

public class ScoreManager : NetworkBehaviourSingleton<ScoreManager>
{
    public delegate void KillAlertDelegate(string killer, string killed);

    public static event KillAlertDelegate KillAlert;

    public struct StatsData
    {
        public int kills;
        public int deaths;
    }

    public Dictionary<string, StatsData> PlayersStats => playersStats;

    private Dictionary<string, StatsData> playersStats;

    private void Awake()
    {
        base.Awake();
        playersStats = new();
        foreach(var player in SessionManager.Instance.PlayersList)
        {
            Debug.LogError(player.Id);
            playersStats.Add(player.Id, new StatsData());
        }
    }

    private void OnEnable()
    {
        SessionManager.Instance.PlayerInLobbyChange += AddPlayerToDictionary;
        SnowBall.OnSnowballHit += OnPlayerHit;
    }

    private void OnDisable()
    {
        SessionManager.Instance.PlayerInLobbyChange -= AddPlayerToDictionary;
        SnowBall.OnSnowballHit -= OnPlayerHit;
    }

    private void AddPlayerToDictionary()
    {
        foreach(var player in SessionManager.Instance.PlayersList)
        {
            if(!playersStats.ContainsKey(player.Id))
            {
                playersStats.Add(player.Id, new StatsData());
            }
        }
    }

    private void OnPlayerHit(string killer, string killed)
    {
        if(playersStats.TryGetValue(killer, out StatsData killerStatsData))
        {
            killerStatsData.kills += 1;
            playersStats[killer] = killerStatsData;
        }
        if (playersStats.TryGetValue(killed, out StatsData KilledStatsData))
        {
            KilledStatsData.deaths += 1;
            playersStats[killed] = KilledStatsData;
        }
        KillAlertRPC(killer, killed);
    }

    [Rpc(target: SendTo.Everyone)]
    private void KillAlertRPC(string killer, string killed)
    {
        string killerUsername = SessionManager.Instance.GetPlayerUsername(killer);
        string killedUsername = SessionManager.Instance.GetPlayerUsername(killed);
        KillAlert?.Invoke(killerUsername, killedUsername);
    }
}
