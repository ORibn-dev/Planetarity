using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameObject gametitle;
    [SerializeField] private GameObject MMbuttons;
    [SerializeField] private Texture2D gamecursor;

    #endregion

    #region SetMainMenuAndItsAnimations

    void Start()
    {
        Cursor.SetCursor(gamecursor, new Vector2(gamecursor.width/2, gamecursor.height/2), CursorMode.Auto);
        Time.timeScale = 1f;
        StartCoroutine(StartMainMenu());
    }

    private IEnumerator StartMainMenu()
    {
        yield return new WaitForSeconds(1f);
        gametitle.SetActive(true);
        gametitle.GetComponent<Animator>().Play("gametitle_in");
        yield return new WaitForSeconds(1f);
        MMbuttons.SetActive(true);
        MMbuttons.GetComponent<Animator>().Play("MMbuttons_in");
    }

    #endregion

    #region MainMenuButtons

    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(2);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion
}
