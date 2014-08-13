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
    }

    void PlanetDestroyed(Planet planet)
    {
        planet.OnShipEnteredRange -= ShipEnteredPlanetRange;
        planet.OnShipExitedRange -= ShipExitedPlanetRange;
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
}
