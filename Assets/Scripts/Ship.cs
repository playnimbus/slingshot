using UnityEngine;
using System.Collections;

// Interface to the ship, manages game logic and delegates movement
public class Ship : MonoBehaviour
{
    private ShipMovement movement;

    public Vector3 direction
    {
        get
        {
            return Vector3.up;
        }
    }

    void Awake()
    {
        movement = GetComponent<ShipMovement>();
    }

    public void UpdateGravityEffect(float angle, float turnSpeed, float speed)
    {
        movement.TurnTowards(angle, turnSpeed);
        movement.SetTurnSpeed(turnSpeed);
        movement.SetSpeed(speed);
    }
}