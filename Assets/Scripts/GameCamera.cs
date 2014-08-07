using UnityEngine;
using System;
using System.Collections;

// Interface into controlling the camera
public class GameCamera : MonoBehaviour
{
    private GameCameraFollow follow;
    private GameCameraOrbit orbit;

    void Awake()
    {
        follow = GetComponent<GameCameraFollow>();
        orbit = GetComponent<GameCameraOrbit>();
    }

    public void FollowShip(Ship ship)
    {
        orbit.Deactivate();
        follow.Activate(ship);        
    }

    public void ShowPlanet(Planet planet)
    {
        follow.Deactivate();
        orbit.Activate(planet);
    }
}