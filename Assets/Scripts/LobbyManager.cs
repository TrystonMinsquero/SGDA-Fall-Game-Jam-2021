using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;

    public Canvas lobby;
    public Canvas howToPlay;
    public PlayerManager playerManager;
    public JoinBox[] joinBoxes;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        UpdateJoinBoxes();
    }

    public void UpdateJoinBoxes()
    {
        for(int i = 0; i < joinBoxes.Length; i++)
        {
            if (PlayerManager.players[i] != null && !joinBoxes[i].hasPlayer)
                joinBoxes[i].AddPlayer(PlayerManager.players[i]);
            else if (PlayerManager.players[i] == null && joinBoxes[i].hasPlayer)
                joinBoxes[i].RemovePlayer(PlayerManager.players[i]);
        }
    }

    public void SwitchToHTP()
    {
        howToPlay.enabled = true;
        lobby.enabled = false;
    }
    public void SwitchToLobby()
    {
        howToPlay.enabled = false;
        lobby.enabled = true;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Road Crossing");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
