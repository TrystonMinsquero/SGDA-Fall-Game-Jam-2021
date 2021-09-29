using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardSlot : MonoBehaviour
{
    public Text playerNum;
    public Text kills;
    public Text deaths;



    public void Fill(Score score)
    {
        playerNum.text = "" + (score.playerIndex + 1);
        kills.text = "" + score.playerKills;
        deaths.text = "" + score.timesDied;
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

}
