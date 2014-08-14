using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;
    [Range(0, 1)] public float accelerationLerp;
    [Range(0, 1)] public float decelerationLerp;

    public float speed;

    private float angle;

    void Start()
    {
        speed = minSpeed;
        StartCoroutine(MovementCoroutine(Mathf.PI / 2f));
    }
    
	IEnumerator MovementCoroutine(float angle)
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        while(true)
        {
            speed = Mathf.Lerp(speed, minSpeed, decelerationLerp);


            Vector3 positionDelta = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * speed * Time.fixedDeltaTime;
            rigidbody2D.MovePosition(transform.position + positionDelta);
            rigidbody2D.MoveRotation((angle - Mathf.PI / 2f) * Mathf.Rad2Deg);
            yield return wait;
        }
    }

    float AngleDifferenceForDistance(float distance, float radius)
    {

        return 0f;
    }

    IEnumerator OrbitCoroutine(Planet planet)
    {
        /* Don't question any math, just go with it */
        WaitForFixedUpdate wait = new WaitForFixedUpdate();        
        Vector3 positionDiff = transform.position - planet.transform.position;
        float currentAngle = Mathf.Atan2(positionDiff.y, positionDiff.x);
        float startRadius = positionDiff.magnitude;

        // 1 for clockwise, -1 for counter clockwise
        float directionModifier = 1f;
        if (transform.InverseTransformPoint(planet.transform.position).x > 0f)
        {
            directionModifier = -1f;
        }

        float currentRadius = startRadius;
        float lerpModifier = Mathf.Abs(transform.InverseTransformDirection(positionDiff.y, -positionDiff.x, 0f).normalized.x);
        print(lerpModifier);
        float radiusLerp =  (speed * Time.fixedDeltaTime) / (startRadius - planet.orbitRadius);
        radiusLerp *= lerpModifier * lerpModifier;

        bool startedLaunch = false;
        while (true)
        {
            float percent = 1f - ((currentRadius - planet.orbitRadius) / (startRadius - planet.orbitRadius));

            currentRadius = Mathf.Lerp(currentRadius, planet.orbitRadius, radiusLerp);
            float angleDelta = percent * (speed / currentRadius);
            currentAngle += directionModifier * angleDelta * Time.fixedDeltaTime;

            Vector3 position = new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle)) * currentRadius + planet.transform.position;
            Vector3 deltaPosition = position - transform.position;

            rigidbody2D.MovePosition(position);
            rigidbody2D.MoveRotation((Mathf.Atan2(deltaPosition.y, deltaPosition.x) - Mathf.PI / 2f) * Mathf.Rad2Deg);

            if(planet.mouseDown)
            {
                speed = Mathf.Lerp(speed, maxSpeed, accelerationLerp);
                startedLaunch = true;
            }
            else
            {
                speed = Mathf.Lerp(speed, minSpeed, decelerationLerp);
            }
            if(!planet.mouseDown && startedLaunch)
            {
                // Launch
                StopAllCoroutines();
                StartCoroutine(MovementCoroutine(currentAngle + directionModifier * Mathf.PI / 2f));
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
