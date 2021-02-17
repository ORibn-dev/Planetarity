using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGeneration : MonoBehaviour
{
    #region MainFields

    [SerializeField] private Enemy enemy; ////enemy prefab
    [SerializeField] private GameObject player; ////player
    [SerializeField] private GameObject SetGameUI; ////Set Game Screen
    [SerializeField] private Texture planettexture, planetnormalmap; ////Enemy planet texture and normal map
    [SerializeField] private PlanetGravity SunGravity; ////Sun gravity for player rockets
    [Header("Enemy Spawn range")]
    [SerializeField] private float x_range1;
    [SerializeField] private float x_range2;
    [SerializeField] private float y_range1;
    [SerializeField] private float y_range2;

    #endregion

    #region SingletonReferences

    public static EnemyGeneration Instance { get; private set; }
    private RocketInventory RI;
    private Stats stat;

    #endregion

    #region EnemiesRelatedFields

    private int minplayers, maxplayers;
    private InputField IF1, IF2; /////input fields for min and max players
    private List <Enemy> enemies;
    private int rocketindex;

    #endregion

    #region EnemyPositionRelatedFields

    private Vector3 current_pos;
    private List<Vector3> potential_pos = new List<Vector3>();
    private int numberofoverlaps;
    private Collider[] overlapdcoliders;

    #endregion

    #region Initialization

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        RI = RocketInventory.Instance;
        stat = Stats.Instance;
    }

    #endregion

    #region SetCurrentGame

    public void SetGame()
    {
        IF1 = SetGameUI.transform.GetChild(2).GetComponent<InputField>(); ////input min AI players
        IF2 = SetGameUI.transform.GetChild(3).GetComponent<InputField>(); ////input max AI players
        int.TryParse(IF1.text, out minplayers);
        int.TryParse(IF2.text, out maxplayers);

        minplayers = (minplayers == 0 && minplayers > 10) ? 1 : minplayers;
        maxplayers = (maxplayers > 10) ? 10 : maxplayers;

        stat.numberofplayers = Random.Range(minplayers, maxplayers);
        stat.numberofplayers_UI.text = "Enemies left: " + stat.numberofplayers;

        GenerateEnemies();

        Time.timeScale = 1;
        DestroyTime.Instace.allowdestroytimer = true;

        SetGameUI.GetComponent<Animator>().Play("SetGameOut");
        Destroy(SetGameUI, 1f);
    }

    private void SetPlayer()
    {
        player.GetComponent<PlayerFiring>().SetPlayerFiring();
        player.GetComponent<RocketToShoot>().SetRocketToShoot(0);
    }

    #endregion

    #region InstantiateAndConfigureEnemiesAndPlayer

    public void GenerateEnemies()
    {
        SetPlayer();
        enemies = new List<Enemy>();
        for (int i = 0; i < stat.numberofplayers; i++)
        {
            enemies.Add(Instantiate(enemy, CheckOverlaps(), Quaternion.identity));
        }
        for (int x = 0; x < enemies.Count; x++)
        {
            rocketindex = Random.Range(1, 3);
            SetEnemyRockets();
            enemies[x].SetEnemy(planettexture, planetnormalmap);
            enemies[x].GetComponent<RocketToShoot>().SetRocketToShoot(rocketindex, player);
            enemies[x].SetScore += stat.UpdateScore;
            enemies[x].UpdateNumberOfEnemies += stat.UpdateNumberofEnemies;
        }
        CollectGravityRockets();
    }

    #endregion

    #region ConfigureEnemyRockets

    private void SetEnemyRockets()
    {
        switch (rocketindex)
        {
            case 1: RI.PooledRockets(false, 0, RI.rocket_pools[1].pooled_rockets, 
                RI.explosion_pools[1].pooled_explosions); break; //small rocket
            case 2 : RI.PooledRockets(false, 1, RI.rocket_pools[2].pooled_rockets, 
                RI.explosion_pools[2].pooled_explosions); break; //medium rocket
            case 3 : RI.PooledRockets(false, 2, RI.rocket_pools[3].pooled_rockets, 
                RI.explosion_pools[3].pooled_explosions); break; //big rocket
        }
    }

    public void CollectGravityRockets()
    {
        for (int y = 0; y < enemies.Count; y++)
        {
            if (enemies[y] != null)
            {
                enemies[y].GetComponent<PlanetGravity>().CollectRockets(RI.rocket_pools[0].pooled_rockets);
            }
        }
        SunGravity.GetComponent<PlanetGravity>().CollectRockets(RI.rocket_pools[0].pooled_rockets);
    }

    #endregion

    #region FindAvailableEnemyPosition

    private Vector3 CheckOverlaps()
    {
        current_pos = new Vector3(Random.Range(x_range1, x_range2), Random.Range(y_range1, y_range2), 162);
        overlapdcoliders = new Collider[1];
        numberofoverlaps = Physics.OverlapSphereNonAlloc(current_pos, 3f, overlapdcoliders);
        if (numberofoverlaps == 1)
        {
            return CheckOverlaps();
        }
        return current_pos;
    }

    #endregion
}
