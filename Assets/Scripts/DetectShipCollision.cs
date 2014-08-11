using UnityEngine;
using System;

public class DetectShipCollision : MonoBehaviour
{
    public event Action OnCollisionEnter;
    public event Action OnCollisionExit;

    void Start()
    {
        if(collider2D != null)
        {
            collider2D.isTrigger = true;
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if(otherCollider.gameObject.GetComponent<Ship>() != null)
        {
            if (OnCollisionEnter != null) OnCollisionEnter();
        }
    }

    void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.GetComponent<Ship>() != null)
        {
            if (OnCollisionExit != null) OnCollisionExit();
        }
    }
}
