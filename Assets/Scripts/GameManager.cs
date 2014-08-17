using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    private static GameManager _instance;
    public static GameManager instance
    {
        get { return _instance; }
    }

    private SceneManager scene;
    private new GameCamera camera;
    private Ship ship;

    public GameCamera gameCamera { get { return camera; } }

    void Awake()
    {
        _instance = this;
        scene = FindObjectOfType<SceneManager>();
        camera = FindObjectOfType<GameCamera>();
        ship = FindObjectOfType<Ship>();
    }

    void Start()
    {
        scene.OnPlanetCreated += PlanetCreated;
        scene.OnPlanetDestroyed += PlanetDestroyed;
        scene.ship = ship;
        camera.ship = ship;
        camera.ShipExitedPlanetRange(null);
    }

    void OnDestroy()
    {
        _instance = null;
    }
    
    void PlanetCreated(Planet planet)
    {
        planet.OnShipEnteredRange += ShipEnteredPlanetRange;
        planet.OnShipExitedRange += ShipExitedPlanetRange;
        planet.OnShipCollided += ShipCollideWithPlanet;
    }

    void PlanetDestroyed(Planet planet)
    {
        planet.OnShipEnteredRange -= ShipEnteredPlanetRange;
        planet.OnShipExitedRange -= ShipExitedPlanetRange;
        planet.OnShipCollided -= ShipCollideWithPlanet;
    }

    void ShipEnteredPlanetRange(Planet planet)
    {
        ship.EnteredPlanetRange(planet);
        camera.ShipEnteredPlanetRange(planet);
    }

    void ShipExitedPlanetRange(Planet planet)
    {
        ship.ExitedPlanetRange(planet);
        camera.ShipExitedPlanetRange(planet);
    }

    void ShipCollideWithPlanet(Planet planet)
    {
        ship.CrashedIntoPlanet(planet);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

#if UNITY_IPHONE

    void OnGUI()
    {
        if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if(GUI.Button(new Rect(0, 0, 200, 200), "Restart"))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }

#endif
}
