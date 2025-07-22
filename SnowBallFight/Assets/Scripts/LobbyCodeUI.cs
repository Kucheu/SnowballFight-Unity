using UnityEngine;

public class LobbyCodeUI : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI lobbyCodeText;

    private void OnEnable()
    {
        lobbyCodeText.text = SessionManager.Instance.Code;
    }

    public void CopyCode()
    {
        GUIUtility.systemCopyBuffer = SessionManager.Instance.Code;
    }
}
