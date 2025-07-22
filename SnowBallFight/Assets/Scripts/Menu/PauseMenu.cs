using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayCore.Menu;

public class PauseMenu : MonoBehaviour
{

    public static PauseMenu Instance;
    PlayerManager playerManager;

    [SerializeField] GameObject firstCamera;

    bool firstChangeTeam = false;

    private bool menuActive = true;

    private void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (menuActive)
            {
                PageController.Instance.TurnPageOff(PageType.ChangeTeamMenu);
                CloseMenu();
            }
            else
            {
                PageController.Instance.TurnPageOn(PageType.ChangeTeamMenu);
                OpenMenu();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PageController.Instance.activePageType == PageType.none)
            {
                PageController.Instance.TurnPageOn(PageType.PauseMenu);
                OpenMenu();
            }
            else
            {
                if (!PageController.Instance.StepBackPage())
                {
                    PageController.Instance.TurnPageOff(PageController.Instance.activePageType);
                    CloseMenu();
                }
            }
        }
    }


    void CloseMenu()
    {
        menuActive = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OpenMenu()
    {
        menuActive = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void FirstOpen(PlayerManager _playerManager)
    {
        playerManager = _playerManager;
        firstCamera.SetActive(true);
        OpenMenu();
        PageController.Instance.TurnPageOn(PageType.ChangeTeamMenu);
    }

    public void Resume()
    {
        PageController.Instance.TurnPageOff(PageController.Instance.activePageType);
        CloseMenu();
    }

    public void QuitToMenu()
    {
        Debug.LogError("QUIT GAME - NOT IMPLEMENTET");
       // PhotonNetwork.LeaveRoom();

    }
}
