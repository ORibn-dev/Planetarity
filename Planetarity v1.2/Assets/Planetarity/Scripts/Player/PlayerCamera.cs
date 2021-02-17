using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    #region Fields

    [SerializeField] private float mainmenumin;
    [SerializeField] private float mainmenumax;
    [SerializeField] private bool mainmenu;

    private float valueX = 0f;
    private float valueY = 0f;

    #endregion

    #region CameraMovement

    void Update()
    {
        if (mainmenu)
        {
            MoveCamera();
        }
        else
        {
            if (!InGameMenu.Instance.gamepaused)
            {
                MoveCamera();
            }
        }
    }

    private void MoveCamera()
    {
        valueX += Input.GetAxis("Mouse X") * 0.5f;
        valueY -= Input.GetAxis("Mouse Y") * 0.5f;
        valueX = Mathf.Clamp(valueX, mainmenumin, mainmenumax);
        valueY = Mathf.Clamp(valueY, -1f, 1f);
        transform.eulerAngles = new Vector3(valueY, valueX, 0f);
    }

    #endregion
}
