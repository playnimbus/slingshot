using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour
{
    public float speed;
    private float angle;

    void Start()
    {
        StartCoroutine(OrbitCoroutine(null));        
    }
    
	IEnumerator MovementCoroutine()
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        while(true)
        {
            Vector3 positionDelta = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * speed * Time.fixedDeltaTime;
            rigidbody2D.MovePosition(transform.position + positionDelta);
            rigidbody2D.MoveRotation((angle - Mathf.PI / 2f) * Mathf.Rad2Deg);
            yield return wait;
        }
    }

    IEnumerator OrbitCoroutine(Planet planet)
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();        
        float currentAngle = 0f;
        float radius = 2f;
        while (true)
        {
            float angleDelta = speed / radius;
            currentAngle += angleDelta * Time.fixedDeltaTime;
            Vector3 position = new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle)) * radius;
            rigidbody2D.MovePosition(position);
            rigidbody2D.MoveRotation(currentAngle * Mathf.Rad2Deg);
            yield return wait;
        }
    }
}
