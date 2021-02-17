using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    #region Fields

    [SerializeField] private Rigidbody rb;
    private List<Rigidbody> rockets = new List<Rigidbody>();

    private Vector3 direction, force;
    private float distance, forcemagnitude;

    #endregion

    #region AttractingPlayerRockets

    void FixedUpdate()
    {
        if (rockets != null)
        {
            foreach (Rigidbody rock in rockets)
            {
                AttractRockets(rock);
            }
        }
    }

    #endregion

    #region CollectingAndAttractingRockets

    public void CollectRockets(List<Rigidbody> pooledrockets)
    {
        rockets.Clear();
        rockets.AddRange(pooledrockets);
    }

    void AttractRockets(Rigidbody rocketoattract)
    {
        direction = rb.position - rocketoattract.position;
        distance = direction.magnitude;
        forcemagnitude = (rb.mass * rocketoattract.mass) / Mathf.Pow(distance, 2);
        force = direction.normalized * (forcemagnitude/100);
        rocketoattract.AddForce(force);
    }

    #endregion
}
