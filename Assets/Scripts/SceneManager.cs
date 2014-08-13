using UnityEngine;
using System;

public class SceneManager : MonoBehaviour 
{
    public Planet planetPrefab;
    public Ship ship { get; set; }

    public event Action<Planet> OnPlanetCreated;
    public event Action<Planet> OnPlanetDestroyed;

    void Start()
    {
        for (int y = -5; y <= 5; y++)
        {
            for (int x = -5; x <= 5; x++)
            {
                CreateRandomPlanet(x, y);   
            }
        }
    }

    void CreateRandomPlanet( int x, int y)
    {
        if (UnityEngine.Random.value > 0.5f) return;
        Planet planet = Instantiate(planetPrefab, (Vector2)new Vector3(x * 20, y * 20) + UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(0, 10), Quaternion.identity) as Planet;
        if(OnPlanetCreated != null) OnPlanetCreated(planet);
    }

}
