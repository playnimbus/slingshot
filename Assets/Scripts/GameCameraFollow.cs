using UnityEngine;
using System.Collections;

public class GameCameraFollow : MonoBehaviour 
{
    public float positioningTime;
    public float leadDistance;
    public float height;
    [Range(0f, 1f)] public float lateralLerp;

    private bool update;
    private Ship ship;
    
    public void Activate(Ship ship)
    {
        this.ship = ship;
        StartCoroutine(MoveToShipCoroutine());
    }

    public void Deactivate()
    {
        update = false;
    }

    private IEnumerator MoveToShipCoroutine()
    {
        update = false;

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.identity;
        float startHeight = transform.position.z;

        float time = 0;
        while (time < positioningTime)
        {
            time += Time.deltaTime;
            float percent = (float)Utils.CubicEaseOut(time, 0, 1, positioningTime);

            float lerpedHeight = Mathf.Lerp(startHeight, height, percent);
            Vector3 targetPosition = ship.transform.position;
            Vector3 lerpedPosition = Vector3.Lerp(startPosition, targetPosition, percent);
            Quaternion lerpedRotation = Quaternion.Lerp(startRotation, endRotation, percent);

            lerpedPosition.z = lerpedHeight;
            transform.position = lerpedPosition;
            transform.rotation = lerpedRotation;

            yield return null;
        }

        update = true;
    }

    private void Update()
    {
        if (update) FollowShip();        
    }

    private void FollowShip()
    {
        Vector3 position = ship.transform.position;
        position.z = height;
        transform.position = position;
    }
}
