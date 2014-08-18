using UnityEngine;
using System.Collections;

public class LightUpShipTarget : MonoBehaviour
{
    public Renderer rendererToLight;
    public Color color;

    private Ship ship;
    private Color savedColor;


	void Start ()
    {
        ship = GameManager.instance.gameShip;
        ship.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        savedColor = rendererToLight.material.color;
	}
	
    void FixedUpdate()
    {
        if ((transform.position - ship.transform.position).sqrMagnitude < 10000f)
        {
            RaycastHit2D hit = Physics2D.Raycast(ship.transform.position, ship.transform.up, Mathf.Infinity, Physics2D.DefaultRaycastLayers);
            if(hit.collider == this.collider2D)
            {
                rendererToLight.material.color = Color.Lerp(rendererToLight.material.color, color, 0.25f);
            }
            else
            {
                rendererToLight.material.color = Color.Lerp(rendererToLight.material.color, savedColor, 0.25f);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        collider2D.enabled = false;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        collider2D.enabled = true;
    }
}
