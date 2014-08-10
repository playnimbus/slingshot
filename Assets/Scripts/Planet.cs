using UnityEngine;
using System;

// Planets contain information regarding how to slingshot them
public class Planet : MonoBehaviour
{
    public float speedForce;

    private bool shipInRange = false;

    public float radius
    {
        get
        {
            return collider2D.bounds.extents.y;
        }
    }

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
        float distanceSquared = (ship.transform.position - transform.position).sqrMagnitude;
        float force = speedForce / distanceSquared;
        ship.ChangeSpeed(force);
        
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
