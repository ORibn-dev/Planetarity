using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stats : MonoBehaviour
{
    #region Fields

    public Player player;
    public int score, numberofplayers;
    public TextMeshProUGUI score_UI, numberofplayers_UI;

    #endregion

    #region Initialization

    public static Stats Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    #region UpdateGameInfo

    public void UpdateUI()
    {
        score_UI.text = score.ToString();
        numberofplayers_UI.text = "Enemies left: " + numberofplayers;
        player.playerhpUI.fillAmount = player.player_hp / 100;
    }
    public void UpdateScore(int setscore)
    {
        score += setscore;
        score_UI.text = score.ToString();
        score_UI.GetComponent<Animator>().Play("UpdateScore_anim", -1, 0);
        DestroyTime.Instace.timer = 16;
    }
    public void UpdateNumberofEnemies()
    {
        numberofplayers--;
        numberofplayers_UI.text = "Enemies left: " + numberofplayers;
        EndScreen.Instance.ShowWinScreen(numberofplayers, score);
    }

    #endregion
}