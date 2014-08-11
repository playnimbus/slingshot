using UnityEngine;
using System.Collections;

[RequireComponent(typeof(DetectShipCollision))]
public class Coin : MonoBehaviour 
{
    void Start()
    {
        DetectShipCollision collision = GetComponent<DetectShipCollision>();
        collision.OnCollisionEnter += () => Destroy(gameObject);
        collision.OnCollisionExit += () => Destroy(gameObject);
    }
}
