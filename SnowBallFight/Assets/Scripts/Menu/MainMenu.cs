using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayCore.Menu;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private PageType fistPage = PageType.none;
    [SerializeField]
    private PageType lobbyPage;
    [SerializeField]
    private PageType failedConnectionPage;

    [SerializeField]
    private TMP_InputField usernameInput;
    [SerializeField]
    private TMP_InputField sessionCodeInput;

    void Start()
    {
        PageController.Instance.TurnPageOn(fistPage);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!PageController.Instance.StepBackPage())
            {
                Debug.Log("You cant back!");
            }
        }
    }
    
    public async void StartGame()
    {
        PageController.Instance.TurnPageOn(PageType.LoadingMenu);
        bool isConnected = await SessionManager.Instance.StartSession(usernameInput.text);
        PageController.Instance.TurnPageOn(isConnected ? lobbyPage : failedConnectionPage);
    }

    public async void JoinGame()
    {
        PageController.Instance.TurnPageOn(PageType.LoadingMenu);
        bool isConnected = await SessionManager.Instance.JoinLobby(sessionCodeInput.text, usernameInput.text);
        PageController.Instance.TurnPageOn(isConnected ? lobbyPage : failedConnectionPage);
    }

    public void LeaveGame()
    {
        SessionManager.Instance.LeaveSession();
        PageController.Instance.TurnPageOn(PageType.MainMenu);
    }

    public void ChangePage(Page page)
    {
        PageController.Instance.ChangePage(page.previousPageType, page.pageType);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StepBackPage()
    {
        PageController.Instance.StepBackPage();
    }

}
