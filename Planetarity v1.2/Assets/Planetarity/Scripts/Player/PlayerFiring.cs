using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public delegate void RocketFire();

public class PlayerFiring : MonoBehaviour, IShooting
{
    #region MainFields

    [SerializeField] private Image cooldownUI;
    [SerializeField] private GameObject RocketsUI;

    #endregion

    #region RocketShootingRelatedFields

    private bool allowfire = true;
    private float cooldownincrease = 0.24f;
    private int rocket_index;

    private TextMeshProUGUI rocket_label;
    private Image rocket_image;
    public event RocketFire Fire;

    #endregion

    #region PlayerFiringSetup

    private RocketInventory RI;

    public void SetPlayerFiring()
    {
        RI = RocketInventory.Instance;
        RI.PooledRockets(true, rocket_index, RI.rocket_pools[0].pooled_rockets, RI.explosion_pools[0].pooled_explosions);
        rocket_label = RocketsUI.GetComponent<TextMeshProUGUI>();
        rocket_image = RocketsUI.transform.GetChild(0).GetComponent<Image>();
        rocket_index = 0;
    }

    #endregion

    #region ShootingAndChangingRockets

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeRocket();
        }

        if (cooldownUI.fillAmount > 0)
        {
            cooldownUI.fillAmount -= (Time.deltaTime / 2f);
        }
        if (!allowfire)
        {
            if (cooldownUI.fillAmount == 0)
            {
                cooldownUI.color = new Color32(101, 134, 248, 255);
                allowfire = true;
            }
        }
    }
    void FixedUpdate()
    {
        if (Input.GetButtonDown("Fire1") && allowfire)
        {
            if (Fire != null)
            {
                Fire();
            }
            Cooldown();
        }
    }
    #endregion

    #region ChangeRocketAndCooldown

    private void ChangeRocket()
    {
        rocket_index = (rocket_index != 2) ? rocket_index + 1 : 0;

        RI.PooledRockets(true, rocket_index, RI.rocket_pools[0].pooled_rockets,
            RI.explosion_pools[0].pooled_explosions);

        EnemyGeneration.Instance.CollectGravityRockets();
        rocket_label.text = RI.playerinv[rocket_index].rocketname;
        cooldownincrease = RI.playerinv[rocket_index].rocketcooldown;
        rocket_image.sprite = RI.playerinv[rocket_index].rocketpic;
    }

    private void Cooldown()
    {
        cooldownUI.fillAmount += cooldownincrease;
        if (cooldownUI.fillAmount >= 1f && allowfire)
        {
            cooldownUI.color = new Color32(206, 88, 229, 255);
            allowfire = false;
        }
    }

    #endregion
}
