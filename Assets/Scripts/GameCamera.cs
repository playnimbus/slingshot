using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour 
{
    public float planetViewHeight;
    public float shipViewHeight;

    public Ship ship { get; set; }

    public void ShipEnteredPlanetRange(Planet planet)
    {
        StopAllCoroutines();
        StartCoroutine(FollowObjectCoroutine(planet.gameObject, planetViewHeight));
    }

    public void ShipExitedPlanetRange(Planet planet)
    {
        StopAllCoroutines();
        StartCoroutine(FollowObjectCoroutine(ship.gameObject, shipViewHeight));
    }

    IEnumerator FollowObjectCoroutine(GameObject @object, float height, float timeToCenter = 1f)
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        Vector3 startPosition = transform.position;
        float time = 0f;

        while (time < timeToCenter)
        {
            time += Time.fixedDeltaTime;
            float percent = time / timeToCenter;

            Vector3 targetPosition = @object.transform.position;
            targetPosition.z = height;
            Vector3 lerpedPosition = Vector3.Lerp(startPosition, targetPosition, percent);
            transform.position = lerpedPosition;

            yield return wait;   
        }


        while(true)
        {
            Vector3 targetPosition = @object.transform.position;
            targetPosition.z = height;
            transform.position = targetPosition;

            yield return wait;
        }
    }
}
