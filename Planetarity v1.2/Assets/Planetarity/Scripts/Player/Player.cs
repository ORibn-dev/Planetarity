using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour, IDamagable
{
    #region Fields

    public float player_hp; //player health
    public Image playerhpUI; //player health UI

    [SerializeField] private float rotationspeed;
    [SerializeField] private GameObject Sun;
    [SerializeField] private GameObject playerexplosion;

    private GameObject playerexplosion_prefab;
    private Quaternion rotation;

    #endregion

    #region PlayerMovement

    void Update()
    {
        rotation = transform.rotation;
        transform.RotateAround(Sun.transform.position, Vector3.forward, -Input.GetAxis("Vertical") * rotationspeed * Time.deltaTime);
        transform.rotation = rotation;
    }

    #endregion

    #region PlayerDamageAndDeath

    public void TakeDamage(float damage, string rocket_tag)
    {
        if (rocket_tag == "EnemyRocket")
        {
            player_hp -= damage;
            playerhpUI.fillAmount -= (damage / 100);
            if (player_hp <= 0)
            {
                PlayerDeath();
            }
        }
    }
    private void PlayerDeath()
    {
        playerexplosion_prefab = Instantiate(playerexplosion, transform.position, transform.rotation) as GameObject;
        Destroy(playerexplosion_prefab, 2f);
        EndScreen.Instance.ShowDeathScreen();
        Destroy(gameObject);
    }

    #endregion
}
