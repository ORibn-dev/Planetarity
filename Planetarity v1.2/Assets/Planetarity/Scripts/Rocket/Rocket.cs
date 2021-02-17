using UnityEngine;

public class Rocket : MonoBehaviour
{
    #region Fields

    public event InitiateExplosions SetExplosion;

    public float rocketdamage, acceleration;
    public int explosion_indx;

    private Collision colobj;
    private IDamagable damageobj;

    #endregion

    #region RocketCollision

    void OnCollisionEnter(Collision col)
    {
        colobj = col;
        RocketCollision(col);
    }

    private void RocketCollision(Collision col)
    {
        damageobj = col.gameObject.GetComponent<IDamagable>();
        if (damageobj != null)
        {
            damageobj.TakeDamage(rocketdamage, gameObject.tag);
        }
        if (EnemyRocket() || PlayerRocket())
        {
            ExplodeRocket();
        }
    }

    private bool EnemyRocket()
    {
        return gameObject.tag == "EnemyRocket" && colobj.gameObject.tag != "Enemy";
    }
    private bool PlayerRocket()
    {
        return gameObject.tag == "Rocket" && colobj.gameObject.tag != "Player";
    }

    #endregion

    #region DestroyThisRocket

    private void ExplodeRocket()
    {
        if (SetExplosion != null)
        {
            SetExplosion(explosion_indx, transform.position, transform.rotation);
        }
        gameObject.SetActive(false);
    }

    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    #endregion
}
