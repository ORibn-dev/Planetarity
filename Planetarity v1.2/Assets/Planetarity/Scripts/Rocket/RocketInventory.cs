using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void InitiateExplosions(int exp_indx, Vector3 exp_pos, Quaternion exp_rot);

public class RocketInventory : MonoBehaviour
{
    #region Pools

    public List<ChooseRocket> playerinv;
    public List<RocketPools> rocket_pools;
    public List<ExplosionPools> explosion_pools;

    #endregion

    #region PooledStuff

    private Rigidbody pooledrocket;
    private ParticleSystem pooledexplosion, availableexplosion;
    private Queue<GameObject> explosionstodisable = new Queue<GameObject>();

    #endregion

    #region Initialization

    public static RocketInventory Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    #endregion

    #region PoolingRocketsAndExplosions

    public void PooledRockets(bool player, int rockindex, List<Rigidbody> chosenpool, 
    List<ParticleSystem> explosionpool)
    {
        if (chosenpool.Count != 0 && player)
        {
           for (int i = 0; i < chosenpool.Count; i++)
           {
               Destroy(chosenpool[i].gameObject, 5f);
               Destroy(explosionpool[i].gameObject, 5f);
           }
           chosenpool.Clear();
           explosionpool.Clear();
        }
        for (int i = 0; i < 10; i++)
        {
            PopulatePools<Rigidbody>(pooledrocket, playerinv[rockindex].rockettoshoot, chosenpool);
            PopulatePools<ParticleSystem>(pooledexplosion, playerinv[rockindex].rocketexp, explosionpool);
        }
    }
    private void PopulatePools<T>(T pooledobj, T objtoinstan, List <T> objpool) where T : Component
    {
        pooledobj = (T)Instantiate(objtoinstan);
        pooledobj.gameObject.SetActive(false);
        objpool.Add(pooledobj);
    }

    #endregion

    #region GetSomethingFromPool

    public T GetStuffFromPool<T>(List<T> pooll) where T : Component
    {
        for (int i = 0; i < pooll.Count; i++)
        {
            if (!pooll[i].gameObject.activeInHierarchy)
            {
                return pooll[i];
            }
        }
        return default(T);
    }

    #endregion

    #region GetAndSetExplosions

    public void InitiateExplosion(int explosion_indx, Vector3 pos, Quaternion rot)
    {
        StartCoroutine(InititateExplosionProccess(explosion_indx, pos, rot));
    }
    private IEnumerator InititateExplosionProccess(int explosion_indx, Vector3 pos, Quaternion rot)
    {
        availableexplosion = GetStuffFromPool<ParticleSystem>(explosion_pools[explosion_indx].pooled_explosions);
        availableexplosion.transform.position = pos;
        availableexplosion.transform.rotation = rot;
        availableexplosion.gameObject.SetActive(true);
        availableexplosion.Play();
        explosionstodisable.Enqueue(availableexplosion.gameObject);
        yield return new WaitForSeconds(2f);
        explosionstodisable.Dequeue().gameObject.SetActive(false);
    }

    #endregion
}
[Serializable]
public struct ChooseRocket
{
    public Sprite rocketpic;
    public string rocketname;
    public Rigidbody rockettoshoot;
    public float rocketcooldown;
    public ParticleSystem rocketexp;
}
[Serializable]
public struct RocketPools
{
    public List<Rigidbody> pooled_rockets;
}
[Serializable]
public struct ExplosionPools
{
    public List<ParticleSystem> pooled_explosions;
}