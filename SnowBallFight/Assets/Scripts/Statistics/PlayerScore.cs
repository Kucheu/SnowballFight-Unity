using UnityEngine;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    [SerializeField]
    private TMP_Text usernameText;
    [SerializeField]
    private TMP_Text killsText;
    [SerializeField]
    private TMP_Text deathText;

    public void SetUsername(string username)
    {
        usernameText.text = username;
    }

    public void SetStats(int killsValue, int deathsValue)
    {
        killsText.text = killsValue.ToString();
        deathText.text = deathsValue.ToString();
    }
}
