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
        Vector3 targetPosition = GetCameraPosition(planet);
        float time = 0f;

        while (time < planetTimeToCenter)
        {
            time += Time.fixedDeltaTime;
            float percent = CubicEaseInOut(time, 0f, 1f, planetTimeToCenter);

            targetPosition.z = planetViewHeight;
            Vector3 lerpedPosition = Vector3.Lerp(startPosition, targetPosition, percent);
            transform.position = lerpedPosition;

            yield return wait;   
        }
        
        while(true)
        {
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

    #region Rectangle

    private Vector3 GetCameraPosition(Planet planet)
    {
        Vector3 position = new Vector3();
        position.z = planetViewHeight;
        position.x = planet.transform.position.x;
        position.y = planet.transform.position.y - (planet.orbitRadius + 1f) + Height(planetViewHeight) / 2f;
        return position;
    }

    public Rect GetRect(Planet planet, bool debugDraw = false)
    {
        Vector3 position = GetCameraPosition(planet);        
        float left = position.x - Width(planetViewHeight) / 2f;
        float top = position.y - Height(planetViewHeight) / 2f;
        float width = Width(planetViewHeight);
        float height = Height(planetViewHeight);

        Rect rect = new Rect(left, top, width, height);
        if (debugDraw)
        {
            Debug.DrawLine(rect.min, rect.min + Vector2.up * height, Color.cyan, 5f);
            Debug.DrawLine(rect.min, rect.min + Vector2.right * width, Color.cyan, 5f);
            Debug.DrawLine(rect.max, rect.max - Vector2.up * height, Color.cyan, 5f);
            Debug.DrawLine(rect.max, rect.max - Vector2.right * width, Color.cyan, 5f);
        }
        return rect;
    }

    private float Height(float cameraHeight)
    {
        float angle = camera.fieldOfView * Mathf.Deg2Rad / 2f;
        return Mathf.Abs(cameraHeight * Mathf.Tan(angle) * 2f);
    }

    private float Width(float cameraHeight)
    {
        float angle = Mathf.Atan(Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad / 2f) * camera.aspect);
        return Mathf.Abs(cameraHeight * Mathf.Tan(angle) * 2f);
    }

    #endregion
}
