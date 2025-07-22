using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Multiplayer;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System;
using Unity.Netcode;
using System.Threading.Tasks;

public class SessionManager : MonoBehaviourSingleton<SessionManager>
{
    public event Action PlayerInLobbyChange;

    internal string GetOwnPlayerID()
    {
        return CurrentSession.CurrentPlayer.Id;
    }
    internal string GetPlayerID(ulong ownerClientId)
    {
        return "TEMPORARY NAME";
    }

    internal string GetPlayerUsername(ulong playerClientID)
    {
        return GetPlayerUsername(GetPlayerID(playerClientID));
    }

    internal string GetPlayerUsername(string playerID)
    {
        foreach(var player in PlayersList)
        {
            if(player.Id == playerID)
            {
                return player.Properties[SessionManager.PlayerNamePropertyKey].Value;
            }
        }
        return "TEST";
        //return String.Empty;
    }

    private const int MaxPlayerCount = 10;
    public const string PlayerNamePropertyKey = "playerName";

    private ISession CurrentSession
    {
        get => currentSession;
        set
        {
            currentSession = value;
        }
    }

    public string Code {
        get
        {
            return CurrentSession != null ? CurrentSession.Code : null;
        }
    }

    public IReadOnlyList<IReadOnlyPlayer> PlayersList
    {
        get
        {
            return CurrentSession != null ? CurrentSession.Players : null;
        }
    }

    private ISession currentSession;


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }


    private async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }


    public async Task<bool> StartSession(string playerName)
    {
        Dictionary<string, PlayerProperty> playerProperties = await SetPlayerProperties(playerName);

        try
        {
            SessionOptions sessionOptions = new SessionOptions
            {
                MaxPlayers = MaxPlayerCount,
                IsPrivate = true,
                IsLocked = false,
                PlayerProperties = playerProperties,
            }.WithRelayNetwork();

            CurrentSession = await MultiplayerService.Instance.CreateSessionAsync(sessionOptions);
            SubscribeToEvents();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return false;
        }
    }

    public async Task<bool> JoinLobby(string sessionCode, string playerName)
    {
        Dictionary<string, PlayerProperty> playerProperties = await SetPlayerProperties(playerName);
        try
        {
            CurrentSession = await MultiplayerService.Instance.JoinSessionByCodeAsync(sessionCode, new JoinSessionOptions
            {
                PlayerProperties = playerProperties
            });
            SubscribeToEvents();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return false;
        }
        return true;
    }
    

    public async void LeaveSession()
    {
        if (CurrentSession != null)
        {
            try
            {
                await CurrentSession.LeaveAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                CurrentSession = null;
            }
        }
    }

    private void SubscribeToEvents()
    {
        CurrentSession.PlayerJoined += OnPlayerJoin;
        CurrentSession.PlayerHasLeft += OnPlayerLeaving;
    }

    private void OnPlayerJoin(string playerName)
    {
        PlayerInLobbyChange?.Invoke();
    }

    private void OnPlayerLeaving(string playerName)
    {
        PlayerInLobbyChange?.Invoke();
    }

    private async Task<Dictionary<string, PlayerProperty>> SetPlayerProperties(string playerName)
    {
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
        }
        PlayerProperty playerNameProperty = new PlayerProperty(playerName, VisibilityPropertyOptions.Member);
        return new Dictionary<string, PlayerProperty> { { PlayerNamePropertyKey, playerNameProperty } };
    }


}
