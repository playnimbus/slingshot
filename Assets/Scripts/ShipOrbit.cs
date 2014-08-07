using UnityEngine;
using System.Collections;

// Handles moving the ship around a planet or slingshotting
public class ShipOrbit : MonoBehaviour
{
    public float degreesPerSecond;
    public float positioningTime;

    private Planet planet;
    private float angle;
    private bool update;
    private int directionModifier = 1;
    private Vector3 mouseStartPosition;

    public void OrbitPlanet(Planet planet)
    {
        this.planet = planet;

        mouseStartPosition.z = -1f;

        Vector3 diff = transform.position - planet.transform.position;
        angle = Mathf.Atan2(diff.y, diff.x);

        Vector3 tangent = Utils.PerpindicularVector(diff);
        float angleCW = Vector3.Angle(transform.up, tangent);
        float angleCCW = Vector3.Angle(transform.up, -tangent);

        if (angleCW > angleCCW) directionModifier = 1;
        else directionModifier = -1;

        StartCoroutine(MoveToPositionCoroutine());
    }

    private IEnumerator MoveToPositionCoroutine()
    {
        update = false;

        float startRadius = Vector3.Distance(transform.position, planet.transform.position);

        float time = 0;
        while(time < positioningTime)
        {
            time += Time.deltaTime;
            float percent = (float)Utils.CubicEaseOut(time, 0, 1, positioningTime);

            float easedRadius = Mathf.Lerp(startRadius, planet.radius, percent);
            angle += degreesPerSecond * directionModifier * Mathf.Deg2Rad * Time.deltaTime;
            Vector3 shipToPlanetNormal = Utils.Vector2FromRadians(angle);
            Vector3 position = shipToPlanetNormal * easedRadius + planet.transform.position;

            Vector3 deltaPosition = position - transform.position;
            transform.position = position;
            transform.up = deltaPosition;

            yield return null;   
        }

        update = true;
    }

    void Update()
    {
        if(update)
        {
            HandleInput();
            Move();
        }
    }

    void HandleInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            mouseStartPosition = Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(0) && mouseStartPosition.z >= 0f)
        {
            Vector3 direction = Input.mousePosition - mouseStartPosition;
            if (direction.magnitude > 50f)
            {
                Vector3 shipToPlanet = planet.transform.position - transform.position;
                if (Vector3.Angle(direction, shipToPlanet) > 90 && Vector3.Angle(direction, transform.up) < 90)
                {
                    GameManager.instance.LaunchInDirection(direction.normalized);
                }
            }
            mouseStartPosition.z = -1f;
        }
    }

    void Move()
    {
        angle += degreesPerSecond * directionModifier * Mathf.Deg2Rad * Time.deltaTime;
        Vector3 shipToPlanetNormal = Utils.Vector2FromRadians(angle);
        Vector3 position = shipToPlanetNormal * planet.radius + planet.transform.position;

        Vector3 deltaPosition = position - transform.position;
        transform.position = position;
        transform.up = deltaPosition;
    }
}
