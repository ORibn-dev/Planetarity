using UnityEngine;
using TMPro;

public class DestroyTime : MonoBehaviour
{
    #region Fields

    public bool allowdestroytimer;
    public float timer;
    [SerializeField]
    private TextMeshProUGUI destroytimerUI;

    #endregion

    #region Initialization

    public static DestroyTime Instace { get; private set; }

    void Awake()
    {
        Instace = this;
    }

    #endregion

    #region UpdateTimer

    void Update()
    {
        if (allowdestroytimer)
        {
            timer -= Time.deltaTime;
            if (timer > 0)
                destroytimerUI.text = "You have " + (int)timer + " seconds to destroy an enemy planet";
            else
                EndScreen.Instance.ShowDeathScreen();
        }
    }

    #endregion
}
