using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameObject WinScreen;
    [SerializeField] private GameObject DeathScreen;
    private TextMeshProUGUI WinScreenScore;

    #endregion

    #region Initialization

    public static EndScreen Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        WinScreenScore = WinScreen.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
    }

    #endregion

    #region GameScreens

    public void ShowWinScreen(int numberofplayers, int score)
    {
        if (numberofplayers == 0)
        {
            WinScreenScore.text = "Your score: " + score.ToString();
            SetScreen(WinScreen, "WinScreen_in");
        }
    }

    public void ShowDeathScreen()
    {
        SetScreen(DeathScreen, "GameOver_in");
    }

    private void SetScreen(GameObject screen, string animation)
    {
        screen.SetActive(true);
        screen.GetComponent<Animator>().Play(animation);
        DestroyTime.Instace.allowdestroytimer = false;
    }

    #endregion
}
