using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public Canvas lobby;
    public Canvas howToPlay;
    
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
