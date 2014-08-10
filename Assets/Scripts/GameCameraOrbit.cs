using UnityEngine;
using System.Collections;

public class GameCameraOrbit : MonoBehaviour
{
    public float positioningTime;
    public float planetBufferSize;
    public float shipBufferSize;

    private Planet planet;
    private bool update;

    public void Activate(Planet planet)
    {
        this.planet = planet;
        StartCoroutine(MoveToPositionCoroutine());
    }

    public void Deactivate()
    {
        planet = null;
        update = false;
    }

    private IEnumerator MoveToPositionCoroutine()
    {
        update = false;

        Vector3 startPosition = transform.position;

        float time = positioningTime;
        while (time > 0f)
        {
            time -= Time.deltaTime;
            float percent = (float)Utils.CubicEaseOut(positioningTime - time, 0, 1, positioningTime);

            Vector3 lerpedPosition = Vector3.Lerp(startPosition, CalculateDesiredPosition(), percent);
            transform.position = lerpedPosition;

            yield return null;
        }

        update = true;
    }

    void Update() 
    {
        if (update) transform.position = Vector3.Lerp(transform.position, CalculateDesiredPosition(), 0.1f);
    }

    private Vector3 CalculateDesiredPosition()
    {
        if (planet == null) return transform.position;
        
        Ship ship = GameManager.instance.ship;
        
        float x = planet.transform.position.x;
        float y = (planet.transform.position.y + planet.radius + ship.transform.position.y) / 2f;

        float heightFit = HeightFit(Mathf.Abs(planet.transform.position.y - ship.transform.position.y) + planet.radius + planetBufferSize);
        float widthFit = WidthFit((Mathf.Abs(planet.transform.position.x - ship.transform.position.x) + shipBufferSize) * 2f);
        float planetWidthFit = WidthFit((planet.radius + planetBufferSize) * 2f);
        float z = Mathf.Min(heightFit, widthFit, planetWidthFit);

        return new Vector3(x, y, z);
    }

    private float HeightFit(float ySpread)
    {
        float angle = camera.fieldOfView * Mathf.Deg2Rad / 2f;
        return -((ySpread / 2f) / Mathf.Tan(angle));
    }

    private float WidthFit(float xSpread)
    {
        float angle = Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad / 2f) * camera.aspect;
        return -((xSpread / 2f) / angle);
    }
}
