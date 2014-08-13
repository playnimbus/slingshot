using UnityEngine;
using System;

public class Planet : MonoBehaviour
{
    public event Action<Planet> OnShipEnteredRange;
    public event Action<Planet> OnShipExitedRange;

    public float orbitRadius;

    private bool _mouseDown = false;
    public bool mouseDown { get { return _mouseDown; } }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Ship ship = otherCollider.gameObject.GetComponent<Ship>();
        if (ship != null)
        {
            if (OnShipEnteredRange != null) OnShipEnteredRange(this);
        }
    }

    void OnTriggerExit2D(Collider2D otherCollider)
    {
        Ship ship = otherCollider.gameObject.GetComponent<Ship>();
        if (ship != null)
        {
            if (OnShipExitedRange != null) OnShipExitedRange(this);
        }
    }

    void OnMouseDown()
    {
        _mouseDown = true;
    }

    void OnMouseUp()
    {
        _mouseDown = false;
    }

    
}
