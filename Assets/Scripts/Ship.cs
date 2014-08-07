using UnityEngine;
using System.Collections;

// Interface to the ship, manages game logic and delegates movement
public class Ship : MonoBehaviour
{
    private ShipMovement movement;
    private ShipOrbit orbit;

    public Vector3 direction
    {
        get
        {
            return movement.direction;
        }
    }

    void Awake()
    {
        movement = GetComponent<ShipMovement>();
        orbit = GetComponent<ShipOrbit>();
    }

    public void OrbitPlanet(Planet planet)
    {
        movement.enabled = false;
        orbit.enabled = true;
        orbit.OrbitPlanet(planet);
    }

    public void MoveInDirection(Vector3 direction)
    {
        orbit.enabled = false;
        movement.enabled = true;
        movement.MoveInDirection(direction);
    }
}