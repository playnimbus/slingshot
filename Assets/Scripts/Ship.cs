using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour
{
    public float speed;

    void Start()
    {
        StartCoroutine(MovementCoroutine(0));        
    }
    
	IEnumerator MovementCoroutine(float angle)
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        while(true)
        {
            speed -= Time.fixedDeltaTime;
            speed = Mathf.Max(speed, 5f);
            Vector3 positionDelta = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * speed * Time.fixedDeltaTime;
            rigidbody2D.MovePosition(transform.position + positionDelta);
            rigidbody2D.MoveRotation((angle - Mathf.PI / 2f) * Mathf.Rad2Deg);
            yield return wait;
        }
    }

    IEnumerator OrbitCoroutine(Planet planet)
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();        
        Vector3 positionDiff = transform.position - planet.transform.position;
        float currentAngle = Mathf.Atan2(positionDiff.y, positionDiff.x);
        float startRadius = positionDiff.magnitude;
        float time = 0f;

        bool startedLaunch = false;

        while(time < 2f)
        {
            time += Time.fixedDeltaTime;

            float lerpedRadius = Mathf.Lerp(startRadius, planet.orbitRadius, time/2f);
            float angleDelta = speed / lerpedRadius;
            currentAngle += angleDelta * Time.fixedDeltaTime;

            Vector3 position = new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle)) * lerpedRadius + planet.transform.position;
            Vector3 deltaPosition = position - transform.position;

            rigidbody2D.MovePosition(position);
            rigidbody2D.MoveRotation((Mathf.Atan2(deltaPosition.y, deltaPosition.x) - Mathf.PI / 2f) * Mathf.Rad2Deg);

            if(planet.mouseDown)
            {
                speed += Time.fixedDeltaTime * 4f;
                startedLaunch = true;
            }
            if(!planet.mouseDown && startedLaunch)
            {
                // Launch
                StopAllCoroutines();
                StartCoroutine(MovementCoroutine(currentAngle + Mathf.PI / 2f));
            }

            yield return wait;
        }

        while (true)
        {
            float angleDelta = speed / planet.orbitRadius;
            currentAngle += angleDelta * Time.fixedDeltaTime;
            Vector3 position = new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle)) * planet.orbitRadius + planet.transform.position;
            
            rigidbody2D.MovePosition(position);
            rigidbody2D.MoveRotation(currentAngle * Mathf.Rad2Deg);

            if (planet.mouseDown)
            {
                speed += Time.fixedDeltaTime * 4f;
                startedLaunch = true;
            }
            if (!planet.mouseDown && startedLaunch)
            {
                // Launch
                StopAllCoroutines();
                StartCoroutine(MovementCoroutine(currentAngle + Mathf.PI / 2f));
            }

            yield return wait;
        }
    }

    public void EnteredPlanetRange(Planet planet)
    {
        StopAllCoroutines();
        StartCoroutine(OrbitCoroutine(planet));
    }

    public void ExitedPlanetRange(Planet planet)
    {

    }

}
