using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    private struct ScoreRowData
    {
        public string PlayerID;
        public PlayerScore scoreRowObject;
        public TeamType teamType;

        public ScoreRowData(string PlayerID, PlayerScore scoreRowObject, TeamType teamType)
        {
            this.PlayerID = PlayerID;
            this.scoreRowObject = scoreRowObject;
            this.teamType = teamType;
        }
    }


    [SerializeField]
    private PlayerScore scorePrefab;
    [SerializeField]
    private Transform blueSide;
    [SerializeField]
    private Transform redSite;
    [SerializeField]
    private GameObject scoreboard;
    [SerializeField]
    private PlayerManager playerManager;

    private Dictionary<string, ScoreRowData> playersData;

    private void Awake()
    {
        playersData = new();
        playerManager = FindAnyObjectByType<PlayerManager>();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
            ShowScoreboard();
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreboard.SetActive(false);
        }
    }

    void ShowScoreboard()
    {
        foreach(var playerStats in ScoreManager.Instance.PlayersStats)
        {
            if(playersData.ContainsKey(playerStats.Key))
            {
                playersData[playerStats.Key].scoreRowObject.SetStats(playerStats.Value.kills, playerStats.Value.deaths);
            }
            else
            {
                TeamType playerTeam = GetPlayerTeam(playerStats.Key);
                PlayerScore newScoreObjet = Instantiate(scorePrefab, playerTeam == TeamType.RedTeam ? redSite : blueSide);
                newScoreObjet.SetUsername(SessionManager.Instance.GetPlayerUsername(playerStats.Key));
                newScoreObjet.SetStats(0, 0);
                playersData.Add(playerStats.Key, new ScoreRowData(playerStats.Key, newScoreObjet, playerTeam));
            }
        }
    }

    private TeamType GetPlayerTeam(string playerID)
    {
        return playerManager.GetPlayerTeam(playerID);
    }
}
