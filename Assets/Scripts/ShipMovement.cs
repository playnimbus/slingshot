using UnityEngine;
using System.Collections;

// Handles moving the ship forward in one direction
public class ShipMovement : MonoBehaviour
{
    [Range(0, 1)] public float accelerationModifier;
    public float minSpeed = 1f;
    public float maxSpeed = 10f;
    
    private float speed = 5f;
    private float angle = Mathf.PI / 2f;
    private float angleDelta = 0f;
    
    private void FixedUpdate()
    {
        HandleInput();
        Move();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 viewportPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            if (viewportPoint.x < 0.5f)
            {
                angleDelta += accelerationModifier * Time.fixedDeltaTime;
            }
            else
            {
                angleDelta -= accelerationModifier * Time.fixedDeltaTime;
            }
        }
        else
        {
            angleDelta = Mathf.MoveTowards(angleDelta, 0, accelerationModifier * Time.fixedDeltaTime);
        }

        angleDelta = Mathf.Clamp(angleDelta, -Time.fixedDeltaTime * Mathf.PI / 2f, Time.fixedDeltaTime * Mathf.PI / 2f);
        angle += angleDelta;
        while (angle < 0f) angle += Mathf.PI * 2;
        while (angle > Mathf.PI * 2) angle -= Mathf.PI * 2;
    }

    private void Move()
    {
        float x = Mathf.Cos(angle);
        float y = Mathf.Sin(angle);
        Vector3 positionDelta = new Vector3(x, y);
        positionDelta *= speed * Time.fixedDeltaTime;

        rigidbody2D.MovePosition(transform.position + positionDelta);
        transform.up = positionDelta;
    }

    public void SetSpeedModifier(float value)
    {
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
    }
}
