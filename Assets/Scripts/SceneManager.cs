using UnityEngine;
using System;
using System.Collections.Generic;

public class SceneManager : MonoBehaviour 
{
    public Planet planetPrefab;
    public Transform firstPlanetTransform;
    public Ship ship { get; set; }

    public float minVerticalDistance;
    public float boarder;
    public float minDistanceBetweenPlanets;

    public event Action<Planet> OnPlanetCreated;
    public event Action<Planet> OnPlanetDestroyed;

    private List<Planet> planets;

    void Start()
    {
        planets = new List<Planet>();
        OnPlanetCreated += (planet) => planets.Add(planet);
        OnPlanetDestroyed += (planet) => planets.Remove(planet);
        //OnPlanetCreated += (planet) => { if (UnityEngine.Random.value < 0.5f) CreateTargetPlanets(planet); };
        OnPlanetCreated += (planet) => { planet.OnShipEnteredRange += CreateTargetPlanets; };

        Planet initialPlanet = CreatePlanet(firstPlanetTransform.position.x, firstPlanetTransform.position.y);
        //CreateTargetPlanets(initialPlanet);
    }

    private void CreateTargetPlanets(Planet planet)
    {
        //planets.ForEach((p) => { if (p != planet && p.transform.position.y < planet.transform.position.y) { OnPlanetDestroyed(p); Destroy(p.gameObject); } });
        //planets.RemoveAll((p) => { return p != planet && p.transform.position.y < planet.transform.position.y; });

        Rect cameraRect = GameManager.instance.gameCamera.GetRect(planet);
        cameraRect.yMin = planet.transform.position.y;
        
        Rect leftRect = new Rect(cameraRect);
        leftRect.width = leftRect.width / 2;
        Rect rightRect = new Rect(leftRect);
        rightRect.x = rightRect.x + rightRect.width;
        leftRect = ShrinkDimensions(leftRect, boarder);
        rightRect = ShrinkDimensions(rightRect, boarder);

        Planet leftPlanet = planets.Find(p => p != planet && leftRect.Contains(p.transform.position)) ;;
        bool leftHasPlanet = leftPlanet != null;


        Planet rightPlanet = planets.Find(p => p != planet && rightRect.Contains(p.transform.position));
        bool rightHasPlanet = rightPlanet != null;
        

        if (!leftHasPlanet || !rightHasPlanet)
        {
            bool twoPlanets = UnityEngine.Random.value < 0.5f;
            if (!twoPlanets)
            {
                if (!rightHasPlanet && !leftHasPlanet)
                {
                    bool left = UnityEngine.Random.value < 0.5f;
                    if (left) leftPlanet = CreatePlanet(leftRect);
                    else rightPlanet = CreatePlanet(rightRect);
                }
            }
            else
            {

                if (!leftHasPlanet) leftPlanet = CreatePlanet(leftRect);
                if (!rightHasPlanet) rightPlanet = CreatePlanet(rightRect);
            }
        }

        //if (leftPlanet != null) planet.OnShipEnteredRange += p => { CreateTargetPlanets(leftPlanet); };
        //if (rightPlanet != null) planet.OnShipEnteredRange += p => { CreateTargetPlanets(rightPlanet); };
    }

    private Planet CreatePlanet(Rect bounds)
    {
        float x, y;
        bool success;
        int tries = 0;
        do
        {
            if (tries++ > 1000) Debug.Break();

            x = Mathf.Lerp(bounds.xMin, bounds.xMax, UnityEngine.Random.value);
            y = Mathf.Lerp(bounds.yMin, bounds.yMax, UnityEngine.Random.value);

            success = true;
            foreach(var p in this.planets)
            {
                if (p != null && Vector3.Distance(p.transform.position, new Vector3(x, y)) < minDistanceBetweenPlanets)
                    success = false;
            }
        }
        while (!success);

        return CreatePlanet(x, y);
    }
    
    private Planet CreatePlanet(float x, float y)
    {
        Vector3 position = new Vector3(x, y);
        Planet planet = Instantiate(planetPrefab, position, Quaternion.identity) as Planet;
        planet.transform.parent = this.transform;
        if (OnPlanetCreated != null) OnPlanetCreated(planet);
        return planet;
    }
    
    private Rect ShrinkDimensions(Rect rect, float amount)
    {
        rect.xMin = rect.xMin + amount;
        rect.yMin = rect.yMin + amount;
        rect.xMax = rect.xMax - amount;
        rect.yMax = rect.yMax - amount;
        return rect;
    }
}
