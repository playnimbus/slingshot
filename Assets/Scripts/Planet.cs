using UnityEngine;
using System;

// Planets contain information regarding how to slingshot them
public class Planet : MonoBehaviour
{
    public float radius;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ship>() != null)
        {
            GameManager.instance.ReachedPlanet(this);
            collider2D.isTrigger = true;
        }
    }
}
