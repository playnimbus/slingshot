using UnityEngine;
using System.Collections;

// Handles moving the ship forward in one direction
public class ShipMovement : MonoBehaviour
{
    [Range(0, 1)] public float accelerationModifier;
    public float minSpeed = 5f;
    public float minTurnSpeed;

    private float speed = 5f;
    private float angle = Mathf.PI / 2f;

    private float turnSpeed;

    private Plane movementPlane;

    void Start()
    {
        turnSpeed = minTurnSpeed;
        movementPlane = new Plane(Vector3.back, Vector3.zero);
    }
    
    private void FixedUpdate()
    {
        HandleInput();

        angle = WrapAngle(angle);
        Move();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButton(0))
        {
            /*Vector3 viewportPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            
            if (viewportPoint.x < 0.5f)
            {
                angle += turnSpeed * Time.fixedDeltaTime;
            }
            else
            {
                angle -= turnSpeed * Time.fixedDeltaTime;
            }*/

            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (movementPlane.Raycast(inputRay, out distance))
            {
                Vector3 worldPoint = inputRay.GetPoint(distance);
                Vector3 shipToInput = worldPoint - transform.position;
                float angle = Mathf.Atan2(shipToInput.y, shipToInput.x);
                TurnTowards(angle, turnSpeed);
            }
        }
    }

    private float WrapAngle(float angle)
    {
        while (angle < 0f) angle += Mathf.PI * 2;
        while (angle > Mathf.PI * 2) angle -= Mathf.PI * 2;
        return angle;
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

    public void TurnTowards(float angle, float turnSpeed)
    {
        while (angle < this.angle) angle += Mathf.PI * 2f;
        if (angle - this.angle > Mathf.PI) angle -= Mathf.PI * 2f;
        this.angle = Mathf.MoveTowards(this.angle, angle, turnSpeed * Time.fixedDeltaTime);
    }

    public void SetTurnSpeed(float value)
    {
        turnSpeed = Mathf.Max(value, minTurnSpeed);
    }

    public void SetSpeed(float value)
    {
        speed = Mathf.Max(minSpeed, value);
    }
}
