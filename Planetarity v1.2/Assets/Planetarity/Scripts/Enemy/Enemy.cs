using UnityEngine;
using UnityEngine.UI;

public delegate void ScoreUpdater(int scoretoadd);
public delegate void NumberofEnemiesUpdater();

public class Enemy : MonoBehaviour, IDamagable
{
    #region EnemyEvents

    public event ScoreUpdater SetScore;
    public event NumberofEnemiesUpdater UpdateNumberOfEnemies;

    #endregion

    #region EnemyStatsFields

    public float enemyhp; ////enemy health
    public Image enemyhpUI;
    private int scoretoadd;
    private float esize;

    #endregion

    #region PlanetExplosionFields

    [SerializeField] private GameObject planetexplosion;
    private GameObject planetexplosion_prefab;

    #endregion

    #region EnemyInitialSetup

    private RocketInventory RI;
    private Renderer rend;

    void InitialSetup()
    {
        RI = RocketInventory.Instance;
        rend = GetComponent<Renderer>();
    }

    #endregion

    #region EnemyFullSetup

    public void SetEnemy(Texture etexture, Texture enormalmap)
    {
        InitialSetup();
        rend.material = new Material(Shader.Find("Standard"));
        rend.material.EnableKeyword("_NORMALMAP");
        rend.material.mainTexture = etexture;
        rend.material.SetTexture("_BumpMap", enormalmap);
        rend.material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        rend.material.SetFloat("_Metallic", 0f);
        rend.material.SetFloat("_Glossiness", 0.081f);

        esize = Random.Range(0.8f, 1.6f);
        GetComponent<Transform>().localScale = new Vector3(esize, esize, esize);
        enemyhp = 100;
        scoretoadd = Random.Range(50, 120);
        GetComponent<EnemyFiring>().SetEnemyFiring();
    }
    #endregion

    #region EnemyDamageAndDeath

    public void TakeDamage(float damage, string rocket_tag)
    {
        if (rocket_tag == "Rocket")
        {
            enemyhp -= damage;
            enemyhpUI.fillAmount -= (damage / 100);
            if (enemyhp <= 0)
            {
                EnemyDeath();
            }
        }
    }
    private void EnemyDeath()
    {
        SetScore(scoretoadd);
        UpdateNumberOfEnemies();
        planetexplosion_prefab = Instantiate(planetexplosion, transform.position, transform.rotation) as GameObject;
        Destroy(gameObject);
        Destroy(planetexplosion_prefab, 2f);
    }
    #endregion
}
