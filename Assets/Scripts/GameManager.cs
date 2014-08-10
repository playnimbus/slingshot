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
    private Ship _ship;

    public Ship ship { get { return _ship; } }

    private void Awake() 
    {
        _instance = this;

        scene = FindObjectOfType<SceneManager>();
        camera = FindObjectOfType<GameCamera>();
        _ship = FindObjectOfType<Ship>();
	}

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.LoadLevel(Application.loadedLevel);
    }

    private void OnDestroy()
    {
        _instance = null;
    }
    
    public void EnterPlanet(Planet planet)
    {
        camera.ShowPlanet(planet);   
    }

    public void ExitPlanet(Planet planet)
    {
        camera.FollowShip(ship);
    }
}
