using UnityEngine;
using System;

public class ShipObstacle : MonoBehaviour
{
    public event Action<GameObject> OnShipCollided;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ship>() != null)
        {
            if (OnShipCollided != null) OnShipCollided(this.gameObject);
        }
    }
}
