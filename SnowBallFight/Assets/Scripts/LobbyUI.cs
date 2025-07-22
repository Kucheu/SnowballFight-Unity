using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Eflatun.SceneReference;

public class LobbyUI : MonoBehaviour
{
    [SerializeField]
    private Transform playerListParent;
    [SerializeField]
    private RoomListElement roomListElementPrefab;
    [SerializeField]
    private GameObject startButtonObject;

    [SerializeField]
    private SceneReference gameScene;

    private Dictionary<string, RoomListElement> playerList;

    private void OnEnable()
    {
        playerList = new();
        RefreshPlayerList();
        SessionManager.Instance.PlayerInLobbyChange += RefreshPlayerList;
        startButtonObject.SetActive(NetworkManager.Singleton.IsServer);
    }

    private void OnDisable()
    {
        SessionManager.Instance.PlayerInLobbyChange -= RefreshPlayerList;
        RemoveAll();
    }

    private void RemoveAll()
    {
        foreach(var playerElement in playerList)
        {
            Destroy(playerElement.Value.gameObject);
        }
        playerList = new();
    }

    private void RefreshPlayerList()
    {
        List<string> playersID = new();
        foreach (var player in SessionManager.Instance.PlayersList)
        {
            if (!playerList.ContainsKey(player.Id))
            {
                var newRoomListElement = Instantiate(roomListElementPrefab, playerListParent);
                newRoomListElement.Setup(player.Properties[SessionManager.PlayerNamePropertyKey].Value);
                playerList.Add(player.Id, newRoomListElement);
            }
            playersID.Add(player.Id);
        }

        if(playerList.Count != playersID.Count)
        {
            List<string> playersToRemove = new();
            foreach(var player in playerList)
            {
                if(!playersID.Contains(player.Key))
                {
                    playersToRemove.Add(player.Key);
                }
            }

            for(int i = 0; i < playersToRemove.Count; i++)
            {
                var playerElement = playerList[playersToRemove[i]];
                Destroy(playerElement.gameObject);
                playerList.Remove(playersToRemove[i]);
            }
        }
    }

    public void StartGame()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(gameScene.Name, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
