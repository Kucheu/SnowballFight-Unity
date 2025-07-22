using UnityEngine;

public class RoomListElement : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI userNameText;

    public void Setup(string username)
    {
        userNameText.text = username;
    }
}