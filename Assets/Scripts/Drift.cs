using UnityEngine;
using System.Collections;

public class Drift : MonoBehaviour
{

    public float angle;
    public float speed;
	

	void Update () 
    {
        float rads = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(rads);
        float y = Mathf.Sin(rads);
        Vector3 direction = new Vector3(x, y);
        direction *= speed * Time.deltaTime;
        transform.position = transform.position + direction;
	}
}
