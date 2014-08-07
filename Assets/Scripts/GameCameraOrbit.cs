using UnityEngine;
using System.Collections;

public class GameCameraOrbit : MonoBehaviour
{
    public float positioningTime;
    public float height;

    public void Activate(Planet planet)
    {
        StartCoroutine(MoveToPlanetCoroutine(planet));
    }

    public void Deactivate()
    {
        
    }

    private IEnumerator MoveToPlanetCoroutine(Planet planet)
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        float startHeight = transform.position.z;

        float time = positioningTime;
        while (time > 0f)
        {
            time -= Time.deltaTime;
            float percent = (float)Utils.CubicEaseIn(positioningTime - time, 0, 1, positioningTime);// (positioningTime - time) / positioningTime;

            float lerpedHeight = Mathf.Lerp(startHeight, height, percent);
            Vector3 lerpedPosition = Vector3.Lerp(startPosition, planet.transform.position, percent);
            Quaternion lerpedRotation = Quaternion.Lerp(startRotation, Quaternion.identity, percent);

            lerpedPosition.z = lerpedHeight;
            transform.position = lerpedPosition;
            transform.rotation = lerpedRotation;

            yield return null;
        }
    }
}
