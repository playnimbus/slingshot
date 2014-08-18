using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;
    [Range(0, 1)] public float accelerationLerp;
    [Range(0, 1)] public float decelerationLerp;

    public float speed;
    public float direction;


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
        this.direction = directionModifier;

        // currentRadius tracks the ship's current orbit distance
        float currentRadius = startRadius;

        // lerpModifier is used to make sure when the ship is entering at a tangent to the planet it doesn't take a hard turn
        float lerpModifier = Mathf.Abs(transform.InverseTransformDirection(positionDiff.y, -positionDiff.x, 0f).normalized.x);

        // How much to lerp the radius each updat. Calculated to the first update is at the current speed
        float radiusLerp = (lerpModifier * speed * Time.fixedDeltaTime) / (startRadius - planet.orbitRadius);

        bool startedLaunch = false;
        while (true)
        {
            // Percent is how far along between the start radius and orbit radius we are, 0-1. Used to dampen the angular velocity
            float percent = 1f - ((currentRadius - planet.orbitRadius) / (startRadius - planet.orbitRadius));

            // Modify it so the more tangent we are, the greater the angular velocity
            percent = Mathf.Lerp(percent, 1f, 1f - lerpModifier);

            // Lerp the radius so we approach the planet while decelerating
            currentRadius = Mathf.Lerp(currentRadius, planet.orbitRadius, radiusLerp);

            // Angular velocity based on keeping same visual speed, modified by how close we're to the planet.
            float angleDelta = percent * (speed / currentRadius);

            // Now change the angle, direction modifier for cw/ccw
            currentAngle += directionModifier * angleDelta * Time.fixedDeltaTime;

            // Set position based on angle and currentRadius
            Vector3 position = new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle)) * currentRadius + planet.transform.position;

            // DeltaPosition for determining ship heading
            Vector3 deltaPosition = position - transform.position;

            // Move to position and face proper direction
            rigidbody2D.MovePosition(position);
            rigidbody2D.MoveRotation((Mathf.Atan2(deltaPosition.y, deltaPosition.x) - Mathf.PI / 2f) * Mathf.Rad2Deg);
            
            yield return wait;

            if (percent > 0.9f) break;
        }

        while(true)
        {
            float angleDelta = (speed / currentRadius);
            currentAngle += directionModifier * angleDelta * Time.fixedDeltaTime;

            // Set position based on angle and currentRadius
            Vector3 position = new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle)) * currentRadius + planet.transform.position;

            // DeltaPosition for determining ship heading
            Vector3 deltaPosition = position - transform.position;

            // Move to position and face proper direction
            rigidbody2D.MovePosition(position);
            rigidbody2D.MoveRotation((Mathf.Atan2(deltaPosition.y, deltaPosition.x) - Mathf.PI / 2f) * Mathf.Rad2Deg);

            if (planet.mouseDown)
            {
                speed = Mathf.Lerp(speed, maxSpeed, accelerationLerp);
                startedLaunch = true;
                currentRadius = Mathf.MoveTowards(currentRadius, planet.radius, 0.5f * Time.fixedDeltaTime);
            }
            else
            {
                speed = Mathf.Lerp(speed, minSpeed, decelerationLerp);
            }
            if (!planet.mouseDown && startedLaunch)
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

    public void CrashedIntoPlanet(GameObject @object)
    {
        StopAllCoroutines();
        rigidbody2D.AddRelativeForce(Random.insideUnitCircle, ForceMode2D.Impulse);
        rigidbody2D.AddTorque(Random.value, ForceMode2D.Impulse);
    }

}
