using UnityEngine;
using System.Collections;

// Handles moving the ship forward in one direction
public class ShipMovement : MonoBehaviour
{
    public float forwardSpeed;
    public float maxStrafeSpeed;
    [Range(0, 1)] public float strafeAcceleration;

    private float strafeSpeed;

    public Vector3 direction { get; set; }

    // Temoporary setup
    void Awake()
    {
        direction = Vector2.up;
    }

    public void MoveInDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    private void Update()
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
                strafeSpeed = Mathf.Lerp(strafeSpeed, -maxStrafeSpeed, strafeAcceleration);
            }
            else
            {
                strafeSpeed = Mathf.Lerp(strafeSpeed, maxStrafeSpeed, strafeAcceleration);
            }
        }
        else
        {
            strafeSpeed = Mathf.Lerp(strafeSpeed, 0f, strafeAcceleration);
        }
    }

    private void Move()
    {
        Vector2 forwardAmount = direction * forwardSpeed;
        Vector2 strafeAmount = Utils.PerpindicularVector(direction) * strafeSpeed;
        Vector2 deltaPosition = (forwardAmount + strafeAmount) * Time.fixedDeltaTime;

        transform.position = (Vector2)transform.position + deltaPosition;
        //rigidbody2D.MovePosition((Vector2)transform.position + deltaPosition);
        transform.up = deltaPosition;
    }
}
