using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour 
{
    public float planetViewHeight;
    public float planetTimeToCenter;

    public float shipViewHeight;
    public float shipTimeToCenter;

    public Ship ship { get; set; }

    private Plane shipPlane;

    void Start()
    {
        shipPlane = new Plane(Vector3.forward, Vector3.zero);
    }

    public void ShipEnteredPlanetRange(Planet planet)
    {
        StopAllCoroutines();
        StartCoroutine(FollowPlanetCoroutine(planet));
    }

    public void ShipExitedPlanetRange(Planet planet)
    {
        StopAllCoroutines();
        StartCoroutine(FollowShipCoroutine());
    }
    
    IEnumerator FollowShipCoroutine()
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        Vector3 startPosition = transform.position;
        float time = 0f;

        while (time < shipTimeToCenter)
        {
            time += Time.fixedDeltaTime;
            float percent = CubicEaseInOut(time, 0f, 1f, shipTimeToCenter);

            Vector3 targetPosition = ship.transform.position;
            targetPosition.z = shipViewHeight;
            Vector3 lerpedPosition = Vector3.Lerp(startPosition, targetPosition, percent);
            transform.position = lerpedPosition;

            yield return wait;
        }

        while (true)
        {
            Vector3 targetPosition = ship.transform.position;
            targetPosition.z = shipViewHeight;
            transform.position = targetPosition;

            yield return wait;
        }
    }
    
    IEnumerator FollowPlanetCoroutine(Planet planet)
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        Vector3 startPosition = transform.position;
        float time = 0f;

        while (time < planetTimeToCenter)
        {
            time += Time.fixedDeltaTime;
            float percent = CubicEaseInOut(time, 0f, 1f, planetTimeToCenter);

            Vector3 targetPosition = planet.transform.position;
            targetPosition.z = planetViewHeight;
            Vector3 lerpedPosition = Vector3.Lerp(startPosition, targetPosition, percent);
            transform.position = lerpedPosition;

            yield return wait;   
        }

        float planetPressedTime = 0f;
        float timeAcceleration = 1f;
        bool spinStarted = false;

        while(true)
        {
            Vector2 offset = new Vector2();
            if (planet.mouseDown)
            {
                spinStarted = true;
                planetPressedTime += Time.fixedDeltaTime * timeAcceleration * ship.direction;
                timeAcceleration *= 1.01f;
                offset.x = Mathf.Cos(planetPressedTime);
                offset.y = Mathf.Sin(planetPressedTime);
            }
            if(!planet.mouseDown && spinStarted)
            {
                planetPressedTime += Time.fixedDeltaTime * timeAcceleration * ship.direction;
                timeAcceleration = Mathf.Lerp(timeAcceleration, 0f, 0.1f);
                offset.x = Mathf.Cos(planetPressedTime);
                offset.y = Mathf.Sin(planetPressedTime);
            }

            Vector3 targetPosition = planet.transform.position;
            targetPosition.z = planetViewHeight;
            targetPosition.x += offset.x;
            targetPosition.y += offset.y;
            transform.position = targetPosition;

            yield return wait;
        }
    }

    float CubicEaseInOut(float time, float start, float end, float duration)
    {
        if ((time /= duration / 2) < 1)
            return end / 2 * time * time * time + start;

        return end / 2 * ((time -= 2) * time * time + 2) + start;
    }

    Vector3 GetMousePosition(Space space)
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;

        if (shipPlane.Raycast(mouseRay, out distance))
        {
            if (space == Space.Self) return transform.InverseTransformPoint(mouseRay.GetPoint(distance));
            return mouseRay.GetPoint(distance);
        }

        return Vector3.zero;
    }
}
