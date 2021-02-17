using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class SaveLoadGame : MonoBehaviour
{
    #region MainFields

    [SerializeField] private GameObject GameSaved_label;
    private List<GameObject> enemies;
    private Renderer ren;
    private Enemy en;

    #endregion

    #region SavingLoadingRelatedFields

    private BinaryFormatter bf;
    private FileStream sfile;
    private GameData gd;

    #endregion

    #region Initialization

    public static SaveLoadGame Instance { get; private set; }
    private Stats stat;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        stat = Stats.Instance;
    }

    #endregion

    #region SaveGame

    public void SaveProgress()
    {
        bf = new BinaryFormatter();
        sfile = File.Create(Application.persistentDataPath + "/GameData.dat");

        gd = new GameData();
        gd.savedscore = stat.score;
        gd.savednumberofplayers = stat.numberofplayers;
        gd.playerhealth = stat.player.player_hp;
        gd.enemies_health = new List<float>();
        gd.enemies_posx = new List<float>();
        gd.enemies_posy = new List<float>();
        gd.enemies_posz = new List<float>();
        gd.enemies_colorsR = new List<float>();
        gd.enemies_colorsG = new List<float>();
        gd.enemies_colorsB = new List<float>();
        gd.enemies_sizes = new List<float>();
        //////////////////////
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        if (enemies.Count > 0)
        {
            foreach (GameObject ene in enemies)
            {
                gd.enemies_health.Add(ene.GetComponent<Enemy>().enemyhp);
                gd.enemies_posx.Add(ene.transform.position.x);
                gd.enemies_posy.Add(ene.transform.position.y);
                gd.enemies_posz.Add(ene.transform.position.z);
                ren = ene.GetComponent<Renderer>();
                gd.enemies_colorsR.Add(ren.material.color.r);
                gd.enemies_colorsG.Add(ren.material.color.g);
                gd.enemies_colorsB.Add(ren.material.color.b);
                gd.enemies_sizes.Add(ene.GetComponent<Transform>().localScale.x);
            }
        }
        /////////////////////
        bf.Serialize(sfile, gd);
        sfile.Close();
    }
    #endregion

    #region LoadGame

    public void LoadProgress()
    {
        if (File.Exists(Application.persistentDataPath + "/GameData.dat"))
        {
            bf = new BinaryFormatter();
            sfile = File.Open(Application.persistentDataPath + "/GameData.dat", FileMode.Open);
            gd = (GameData)bf.Deserialize(sfile);
            sfile.Close();

            stat.score = gd.savedscore;
            stat.numberofplayers = gd.savednumberofplayers;
            stat.player.player_hp = gd.playerhealth;
            stat.UpdateUI();

            EnemyGeneration.Instance.GenerateEnemies();
            ////////////////////////////////////
            enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

            for (int i = 0; i < enemies.Count; i++)
            {
                en = enemies[i].GetComponent<Enemy>();
                en.enemyhp = gd.enemies_health[i];
                en.enemyhpUI.fillAmount = (gd.enemies_health[i] / 100);
                enemies[i].transform.position = new Vector3(gd.enemies_posx[i],
                    gd.enemies_posy[i], gd.enemies_posz[i]);
                enemies[i].GetComponent<Renderer>().material.color = new Color(gd.enemies_colorsR[i],
                    gd.enemies_colorsG[i], gd.enemies_colorsB[i]);
                enemies[i].transform.localScale = new Vector3(gd.enemies_sizes[i],
                    gd.enemies_sizes[i], gd.enemies_sizes[i]);
            }
        }
        else SceneManager.LoadScene(1);
    }
    #endregion

    #region GameIsSavedNotification

    public void ShowSavedGameL()
    {
        StartCoroutine(SavedGameEnum());
    }
    private IEnumerator SavedGameEnum()
    {
        GameSaved_label.SetActive(true);
        yield return new WaitForSeconds(2f);
        GameSaved_label.GetComponent<Animator>().Play("GameSaved_out", -1, 0);
        yield return new WaitForSeconds(1f);
        GameSaved_label.SetActive(false);
    }
    #endregion
}
[Serializable]
class GameData
{
    public int savedscore;
    public int savednumberofplayers;
    public float playerhealth;
    public List<float> enemies_health;
    public List<float> enemies_posx, enemies_posy, enemies_posz;
    public List<float> enemies_colorsR, enemies_colorsG, enemies_colorsB;
    public List<float> enemies_sizes;
}
