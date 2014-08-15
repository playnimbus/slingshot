using UnityEngine;
using System;

public class DetectShip : MonoBehaviour
{
    public event Action OnShipEnterRange;
    public event Action OnShipExitRange;

	void Start () 
    {
        collider2D.isTrigger = true;
	}

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.GetComponent<Ship>() != null)
        {
            if (OnShipEnterRange != null) OnShipEnterRange();
        }
    }

    void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.GetComponent<Ship>() != null)
        {
            if (OnShipExitRange != null) OnShipExitRange();
        }
    }
}
