using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    private static GameManager _instance;
    public static GameManager instance
    {
        get { return _instance; }
    }

    private SceneManager scene;
    private new GameCamera camera;
    private Ship ship;

    void Awake()
    {
        _instance = this;
        scene = FindObjectOfType<SceneManager>();
        camera = FindObjectOfType<GameCamera>();
        ship = FindObjectOfType<Ship>();
    }

    void Start()
    {
        scene.ship = ship;
        camera.ship = ship;
    }

    void OnDestroy()
    {
        _instance = null;
    }
}
