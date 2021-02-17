using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFiring : MonoBehaviour, IShooting
{
    #region Fields

    [SerializeField] private Image cooldownUI;

    private float firerate, nextfire;
    private float cooldownincrease = 0.24f;
    public event RocketFire Fire;

    #endregion

    #region InitialSetup

    public void SetEnemyFiring()
    {
        firerate = Random.Range(1.5f, 2f);
        nextfire = Time.time;
    }

    #endregion

    #region ShootingRockets

    void Update()
    {
        if (cooldownUI.fillAmount > 0)
        {
            cooldownUI.fillAmount -= (Time.deltaTime / 2f);
        }
    }

    void FixedUpdate()
    {
        if (Time.time > nextfire)
        {
            if (Fire != null)
            {
                Fire();
            }
            cooldownUI.fillAmount += cooldownincrease;
            nextfire = Time.time + firerate;
        }
    }

    #endregion
}
