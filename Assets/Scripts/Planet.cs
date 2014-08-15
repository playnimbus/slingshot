using UnityEngine;
using System;

public class Planet : MonoBehaviour
{
    public event Action<Planet> OnShipEnteredRange;
    public event Action<Planet> OnShipExitedRange;
    public event Action<Planet> OnShipCollided;

    public float orbitRadius;
    public float radius;

    private bool _mouseDown = false;
    public bool mouseDown { get { return _mouseDown; } }

    void Start()
    {
        DetectShip detect = GetComponentInChildren<DetectShip>();
        detect.OnShipEnterRange += () => { if (OnShipEnteredRange != null) OnShipEnteredRange(this); };
        detect.OnShipExitRange += () => { if (OnShipExitedRange != null) OnShipExitedRange(this); };
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Ship>() != null)
        {
            if (OnShipCollided != null) OnShipCollided(this);
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
