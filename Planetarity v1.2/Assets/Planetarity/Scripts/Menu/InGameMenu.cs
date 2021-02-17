using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    #region Fields

    public GameObject InGameMenuUI;
    public bool gamepaused, loadgame;
    private SaveLoadGame SLG;
    public static InGameMenu Instance { get; private set; }

    #endregion

    #region Initialization

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SLG = SaveLoadGame.Instance;

        Time.timeScale = 0;
        if (loadgame)
        {
            LoadGame(false);
            Time.timeScale = 1;
        }
    }

    #endregion

    #region PauseOrResumeGame

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gamepaused)
            {
                PauseOrResumeGame(true);
            }
            else
            {
                PauseOrResumeGame(false);
            }
        }
    }

    public void PauseOrResumeGame(bool pause)
    {
        Time.timeScale = (!pause)? 1 : 0;
        InGameMenuUI.SetActive(pause);
        gamepaused = pause;
    }

    #endregion

    #region InGameMenuButtons

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void TryAgain()
    {
        SceneManager.LoadScene(1);
    }

    public void SaveGame()
    {
        PauseOrResumeGame(false);
        SLG.SaveProgress();
        SLG.ShowSavedGameL();
    }
    public void LoadGame(bool restart)
    {
        if (restart)
            SceneManager.LoadScene(2);
        else
            SLG.LoadProgress();
    }

    #endregion
}
