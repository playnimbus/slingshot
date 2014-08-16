using UnityEngine;
using System;

public class SceneManager : MonoBehaviour 
{
    public Planet planetPrefab;
    public Transform firstPlanetTransform;
    public Ship ship { get; set; }

    public float minDistance;
    public float maxDistance;    
    public float maxAngle;
    public int depth;

    public event Action<Planet> OnPlanetCreated;
    public event Action<Planet> OnPlanetDestroyed;

    void Start()
    {
        Planet initialPlanet = CreatePlanetAt(firstPlanetTransform.position.x, firstPlanetTransform.position.y);
        PlanetNode root = new PlanetNode();
        root.planet = initialPlanet;
        CreatePlanets(root, 0, depth);
    }

    void CreatePlanets(PlanetNode node, int currentDepth, int maxDepth)
    {
        if (currentDepth >= maxDepth) return;
        if (node.north == null)
        {
            PlanetNode north = new PlanetNode();
            north.south = node;
            node.north = north;
            float angle = ((UnityEngine.Random.value - 0.5f) * maxAngle * 2f + 90f) * Mathf.Rad2Deg;
            float distance = Mathf.Lerp(minDistance, maxDistance, UnityEngine.Random.value);
            float x = Mathf.Cos(angle) * distance;
            float y = Mathf.Sin(angle) * distance;
            north.planet = CreatePlanetAt(node.x + x, node.y + y);
            CreatePlanets(north, currentDepth + 1, maxDepth);
        }
        if (node.east == null)
        {
            PlanetNode east = new PlanetNode();
            east.west = node;
            node.east = east;
            float angle = ((UnityEngine.Random.value - 0.5f) * maxAngle * 2f + 0f) * Mathf.Rad2Deg;
            float distance = Mathf.Lerp(minDistance, maxDistance, UnityEngine.Random.value);
            float x = Mathf.Cos(angle) * distance;
            float y = Mathf.Sin(angle) * distance;
            east.planet = CreatePlanetAt(node.x + x, node.y + y);
            CreatePlanets(east, currentDepth + 1, maxDepth);
        }
        if (node.south == null)
        {
            PlanetNode south = new PlanetNode();
            south.north = node;
            node.south = south;
            float angle = ((UnityEngine.Random.value - 0.5f) * maxAngle * 2f + 270f) * Mathf.Rad2Deg;
            float distance = Mathf.Lerp(minDistance, maxDistance, UnityEngine.Random.value);
            float x = Mathf.Cos(angle) * distance;
            float y = Mathf.Sin(angle) * distance;
            south.planet = CreatePlanetAt(node.x + x, node.y + y);
            CreatePlanets(south, currentDepth + 1, maxDepth);
        }
        if (node.west == null)
        {
            PlanetNode west = new PlanetNode();
            west.east = node;
            node.west = west;
            float angle = ((UnityEngine.Random.value - 0.5f) * maxAngle * 2f + 180f) * Mathf.Rad2Deg;
            float distance = Mathf.Lerp(minDistance, maxDistance, UnityEngine.Random.value);
            float x = Mathf.Cos(angle) * distance;
            float y = Mathf.Sin(angle) * distance;
            west.planet = CreatePlanetAt(node.x + x, node.y + y);
            CreatePlanets(west, currentDepth + 1, maxDepth);
        }
    }

    private Planet CreatePlanetAt(float x, float y)
    {
        Vector3 position = new Vector3(x, y);
        Planet planet = Instantiate(planetPrefab, position, Quaternion.identity) as Planet;
        planet.transform.parent = this.transform;
        if (OnPlanetCreated != null) OnPlanetCreated(planet);
        return planet;
    }

    private class PlanetNode
    {
        public Planet planet;
        public PlanetNode north;
        public PlanetNode east;
        public PlanetNode south;
        public PlanetNode west;

        public float x { get { return planet.transform.position.x; } }
        public float y { get { return planet.transform.position.y; } }
    }
}
