using UnityEngine;

public class KillFeedInvoker : MonoBehaviour
{
    [SerializeField]
    private KillFeed killFeed;

    private void OnEnable()
    {
        ScoreManager.KillAlert += OnKillAlert;
    }

    private void OnDisable()
    {
        ScoreManager.KillAlert -= OnKillAlert;
    }

    private void OnKillAlert(string killer, string killed)
    {
        killFeed.SendKillInfo(killed, killer);
    }


#if UNITY_EDITOR || DEVELOPMENT_BUILD
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            killFeed.SendKillInfo("Player 1", "Player 2");
        }
    }
#endif
}
