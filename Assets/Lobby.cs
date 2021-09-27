using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    public Canvas HowToPlay;
    
    public void PlayGame()
    {
        SceneManager.LoadScene("Road Crossing");
    }

    public void howToPlay()
    {
        HowToPlay.enabled = (true);
        this.GetComponent<Canvas>().enabled = false;
    }
}
