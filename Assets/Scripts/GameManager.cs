using UnityEngine;
using System.Collections;
using System;

// Responsible for handling communication between game systems
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager instance
    {
        get { return _instance; }
    }

    private SceneManager scene;
    private GameCamera camera;
    private Ship ship;

    private void Awake() 
    {
        _instance = this;

        scene = FindObjectOfType<SceneManager>();
        camera = FindObjectOfType<GameCamera>();
        ship = FindObjectOfType<Ship>();
	}

    void Start()
    {
        camera.FollowShip(ship);
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    public void ReachedPlanet(Planet planet)
    {
        camera.ShowPlanet(planet);
        ship.OrbitPlanet(planet);
    }

    public void LaunchInDirection(Vector3 direction)
    {        
        ship.MoveInDirection(direction);
        camera.FollowShip(ship); 
    }
}
