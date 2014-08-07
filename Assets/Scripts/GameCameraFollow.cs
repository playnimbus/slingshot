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
        Quaternion endRotation = Quaternion.LookRotation(Vector3.forward, ship.direction);
        float startHeight = transform.position.z;

        float time = positioningTime;
        while (time > 0f)
        {
            time -= Time.deltaTime;
            float percent = (float)Utils.CubicEaseOut(positioningTime - time, 0, 1, positioningTime);

            float lerpedHeight = Mathf.Lerp(startHeight, height, percent);
            Vector3 targetPosition = ship.transform.position + ship.direction * leadDistance;
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
        Vector3 virtualShipPosition = ship.transform.position + ship.direction * leadDistance;
        virtualShipPosition.z = height;

        // Projected position is the nearest point in line with the ship movement
        Vector3 projectedPosition = Vector3.Project(virtualShipPosition - transform.position, ship.direction) + transform.position;
        Vector3 lerpedPosition = Vector3.Lerp(projectedPosition, virtualShipPosition, lateralLerp);

        transform.position = lerpedPosition;
        transform.up = ship.direction;
    }
}
