using UnityEngine;
using System;

// Planets contain information regarding how to slingshot them
public class Planet : MonoBehaviour
{
    public float gravitationalForce;

    private bool shipInRange = false;

    void Start()
    {
        DetectShipCollision collision = GetComponentInChildren<DetectShipCollision>();
        collision.OnCollisionEnter += OnShipEnterRange;
        collision.OnCollisionExit += OnShipExitRange;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ship>() != null)
        {
            OnShipHit();
        }
    }

    void FixedUpdate()
    {
        if(shipInRange)
        {
            ShipInRange();
        }
    }

    void OnShipHit()
    {
        
    }

    void ShipInRange()
    {
        Ship ship = GameManager.instance.ship;
        Vector3 shipToPlanet = transform.position - ship.transform.position;
        float distanceSquared = shipToPlanet.sqrMagnitude;
        float force = gravitationalForce / distanceSquared;
        ship.SetGravityModifier(force);
    }

    void OnShipEnterRange()
    {
        shipInRange = true;
        GameManager.instance.EnterPlanet(this);
    }

    void OnShipExitRange()
    {
        shipInRange = false;
        GameManager.instance.ExitPlanet(this);
    }
}
