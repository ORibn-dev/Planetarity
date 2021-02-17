using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketToShoot : MonoBehaviour
{
    #region MainFields

    [SerializeField] private bool ifenemy;
    [SerializeField] private Transform RocketSpawn;

    #endregion

    #region RocketRelatedFields

    private GameObject player_target; ////for enemies
    private Rigidbody firedrocket;
    private Rocket rocket;
    private int explosion_index;

    #endregion

    #region RocketShootingDirection

    private Vector2 rocket_direction;
    private Vector3 poss; ////mouse click position

    #endregion

    #region InitialSetup

    private RocketInventory RI;

    public void SetRocketToShoot(int rocket_index, GameObject player = null)
    {
        RI = RocketInventory.Instance;
        player_target = player;
        explosion_index = rocket_index;
        GetComponent<IShooting>().Fire += FireRocket;
    }

    #endregion

    #region RocketShooting

    private void FireRocket()
    {
        firedrocket = RI.GetStuffFromPool<Rigidbody>(RI.rocket_pools[explosion_index].pooled_rockets);
        firedrocket.transform.position = RocketSpawn.position;
        firedrocket.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90f));
        firedrocket.gameObject.SetActive(true);

        rocket = firedrocket.GetComponent<Rocket>();
        rocket.SetExplosion -= RI.InitiateExplosion;
        rocket.SetExplosion += RI.InitiateExplosion;
        rocket.explosion_indx = explosion_index;

        if (ifenemyfire())
        {
            firedrocket.tag = "EnemyRocket";
            rocket_direction = player_target.transform.position - RocketSpawn.position;
        }
        else
        {
            poss = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 162f - 148.1f));
            rocket_direction = poss - RocketSpawn.position;
        }
        firedrocket.velocity = rocket_direction.normalized * (rocket.acceleration + 5f);
    }

    private bool ifenemyfire()
    {
        return player_target != null && ifenemy;
    }

    #endregion
}
