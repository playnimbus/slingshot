using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour 
{
    public Transform objectToOrbit;
    public float orbitRadius;
    public float orbitSpeed;

    private float angle;

    void Awake()
    {
        if(objectToOrbit == null && transform.parent != null)
        {
            objectToOrbit = transform.parent;
        }

        transform.parent = null;

        angle = Random.value * Mathf.PI * 2f;

        FixedUpdate();
    }

    void FixedUpdate()
    {
        angle += orbitSpeed * Mathf.Deg2Rad * Time.fixedDeltaTime;
        while (angle < 0f) angle += Mathf.PI * 2f;
        while (angle > Mathf.PI * 2f) angle -= Mathf.PI * 2f;

        float x = Mathf.Cos(angle) * orbitRadius;
        float y = Mathf.Sin(angle) * orbitRadius;
        Vector3 position = objectToOrbit.transform.position + new Vector3(x, y);

        rigidbody2D.MovePosition(position);
        rigidbody2D.MoveRotation(angle * Mathf.Rad2Deg);
    }
}
