using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// Handles moving the ship forward in one direction
public class ShipMovement : MonoBehaviour
{
    public LineRenderer path;
    public float pathSpacing;
    public float maxAngleTurn;

    private Vector3 lastPosition;
    private Vector3 currentPosition;
    private Queue<Vector3> points;
    private Plane movementPlane;
    private float speed = 5f;
    private Vector3[] path;

    private bool followPath = true;

    void Start()
    {
        lastPosition = Vector3.zero;
        currentPosition = Vector3.zero;
        points = new Queue<Vector3>();
        movementPlane = new Plane(Vector3.forward, Vector3.zero);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;

            if (movementPlane.Raycast(ray, out distance))
            {
                Vector3 position = ray.GetPoint(distance);
                if (Input.GetMouseButtonDown(0) || (lastPosition - position).sqrMagnitude > pathSpacing * pathSpacing)
                {
                    if (lastPosition != Vector3.zero) position = (position - lastPosition).normalized * pathSpacing + lastPosition;
                    lastPosition = position;
                    points.Enqueue(position);
                    t = 0f;
                }
            }
        }
        
        Move();
    }

    void OnDrawGizmos()
    {
        if (points != null && points.Count > 2)
        {
            Vector3[] path = points.ToArray();
            iTween.DrawPath(path);
        }
    }

    private IEnumerator BuildPathCoroutine(Vector3 startPosition)
    {
        List<Vector3> path = new List<Vector3>(64);
        path.Add(startPosition);
        Vector3 previousPosition = startPosition;
        float pathSpacingSquared = pathSpacing * pathSpacing;

        while(Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;

            if (movementPlane.Raycast(ray, out distance))
            {
                Vector3 currentPosition = ray.GetPoint(distance);
                if ((currentPosition - lastPosition).sqrMagnitude > pathSpacingSquared)
                {
                    path.Add(currentPosition);
                    lastPosition = currentPosition;
                }
            }

            yield return null;
        }

        if(path.Count >= 2)
        {
            Vector3[] pathPoints = path.ToArray();
            StartCoroutine(MoveOnPathCoroutine(pathPoints));
        }
    }

    private IEnumerator MoveOnPathCoroutine(Vector3[] path)
    {
        while(true)
        {
            yield return null;
        }
    }

    float t;
    private void Move()
    {
        if (followPath)
        {
            if (points.Count > 2 && t <= 1f)
            {
                Vector3 position = transform.position;
                Vector3[] path = points.ToArray();
                float pathLength = iTween.PathLength(path);
                float percentForSpeed = speed / pathLength;
                t += percentForSpeed * Time.deltaTime;
                iTween.PutOnPath(gameObject, path, t);
                float angleTurn = Vector3.Angle(transform.up, transform.position - position);
                if (angleTurn > maxAngleTurn&& angleTurn < 170f)
                {                    
                    print(angleTurn);
                    transform.position = position;
                    followPath = false;
                }
                else  transform.up = transform.position - position;
            }
        }
        else
        {
            transform.position = transform.position + transform.up * speed * Time.deltaTime;
        }     
    }

    public void SetSpeed(float value)
    {
        if (value > 5f) speed = value;
        else speed = 5f;
    }
}
