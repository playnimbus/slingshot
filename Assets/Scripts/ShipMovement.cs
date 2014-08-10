using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// Handles moving the ship forward in one direction
public class ShipMovement : MonoBehaviour
{
    public LineRenderer pathRenderer;
    public float pathSpacing;
    public float maxAngleTurn;

    private Plane movementPlane;
    private float speed = 10f;

    private bool followPath = true;
    private Vector3[] gizmosPath = null;

    void Start()
    {
        movementPlane = new Plane(Vector3.forward, Vector3.zero);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;

            if (movementPlane.Raycast(ray, out distance))
            {
                Vector3 position = ray.GetPoint(distance);
                if(collider2D is BoxCollider2D && (collider2D as BoxCollider2D).OverlapPoint(position))
                {
                    StartCoroutine(BuildPathCoroutine(transform.position));
                }
            }
        }
    }

    private IEnumerator BuildPathCoroutine(Vector3 startPosition)
    {
        List<Vector3> path = new List<Vector3>(64);
        path.Add(startPosition);
        Vector3 previousPosition = startPosition;
        float pathSpacingSquared = pathSpacing * pathSpacing;
        
        pathRenderer.SetVertexCount(1);
        pathRenderer.SetPosition(0, startPosition);

        while(Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;

            if (movementPlane.Raycast(ray, out distance))
            {
                Vector3 currentPosition = ray.GetPoint(distance);
                if ((currentPosition - previousPosition).sqrMagnitude > pathSpacingSquared)
                {
                    path.Add(currentPosition);
                    previousPosition = currentPosition;
                    pathRenderer.SetVertexCount(path.Count);
                    pathRenderer.SetPosition(path.Count - 1, currentPosition);
                }
            }

            yield return null;
        }

        if(path.Count >= 2)
        {
            Vector3[] pathPoints = path.ToArray();
            StartCoroutine(MoveOnPathCoroutine(pathPoints));
            gizmosPath = pathPoints;
        }
    }

    private IEnumerator MoveOnPathCoroutine(Vector3[] path)
    {
        // percent 0 -> 1
        float percent = 0f;
        float pathLength = iTween.PathLength(path);
        path = iTween.PathControlPointGenerator(path);
        bool continueOnPath = true;

        while (continueOnPath && percent <= 1)
        {
            percent += (speed / pathLength) * Time.deltaTime;
            Vector3 position = Approx(path, percent);
            Vector3 newUp = position - transform.position;
            float turnAngle = Vector3.Angle(newUp, transform.up);

            //if(turnAngle < maxAngleTurn)
            {
                transform.up = newUp;
                transform.position = position;
            }
            //else
            {
                //continueOnPath = false;
            }

            yield return null;
        }

        StartCoroutine(MoveStraight());
    }

    private IEnumerator MoveStraight()
    {
        while(true)
        {
            transform.position = transform.position + transform.up * speed * Time.deltaTime;
            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        if(gizmosPath != null)
        {
            iTween.DrawPath(gizmosPath);
        }
    }

    public void SetSpeed(float value)
    {
        if (value > 10f) speed = value;
        else speed = 10f;
    }

    private Vector3 Interp(Vector3[] pts, float percent)
    {
        int numSections = pts.Length - 3;
        int currPt = Mathf.Min(Mathf.FloorToInt(percent * (float)numSections), numSections - 1);
        float u = percent * (float)numSections - (float)currPt;

        Vector3 p0 = pts[currPt];
        Vector3 p1 = pts[currPt + 1];
        Vector3 p2 = pts[currPt + 2];
        Vector3 p3 = pts[currPt + 3];

        float alpha = 1f;

        float t0 = 0;
        float t1 = Mathf.Pow((p1 - p0).magnitude, alpha) + t0;
        float t2 = Mathf.Pow((p2 - p1).magnitude, alpha) + t1;
        float t3 = Mathf.Pow((p3 - p2).magnitude, alpha) + t2;

        float t = Mathf.Lerp(t1, t2, u);

        Vector3 l01 = p0 * ((t1 - t) / (t1 - t0)) + p1 * ((t - t0) / (t1 - t0));
        Vector3 l12 = p1 * ((t2 - t) / (t2 - t1)) + p2 * ((t - t1) / (t2 - t1));
        Vector3 l23 = p2 * ((t3 - t) / (t3 - t2)) + p3 * ((t - t2) / (t3 - t2));

        Vector3 l012 = l01 * ((t2 - t) / (t2 - t0)) + l12 * ((t - t0) / (t2 - t0));
        Vector3 l123 = l12 * ((t3 - t) / (t3 - t1)) + l23 * ((t - t1) / (t3 - t1));

        Vector3 c12 = l012 * ((t2 - t) / (t2 - t1)) + l123 * ((t - t1) / (t2 - t1));

        return c12;
    }

    private Vector3 Approx(Vector3[] pts, float percent)
    {
        int numSections = pts.Length - 3;
        int currPt = Mathf.Min(Mathf.FloorToInt(percent * (float)numSections), numSections - 1);
        float u = percent * (float)numSections - (float)currPt;

        Vector3 p0 = pts[currPt];
        Vector3 p1 = pts[currPt + 1];
        Vector3 p2 = pts[currPt + 2];
        Vector3 p3 = pts[currPt + 3];

        Vector3 cp0 = p1 + (p2 - p0).normalized * (pathSpacing / 4f);
        Vector3 cp1 = p2 + (p1 - p3).normalized * (pathSpacing / 4f);

        Vector3 l0 = Vector3.Lerp(p1, cp0, u);
        Vector3 l1 = Vector3.Lerp(cp0, cp1, u);
        Vector3 l2 = Vector3.Lerp(cp1, p2, u);

        Vector3 c0 = Vector3.Lerp(l0, l1, u);
        Vector3 c1 = Vector3.Lerp(l1, l2, u);

        Vector3 f = Vector3.Lerp(c0, c1, u);
        return f;
    }
}
