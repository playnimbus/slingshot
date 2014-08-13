using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    private static GameManager _instance;
    public static GameManager instance
    {
        get { return _instance; }
    }

    private SceneManager _scene;
    private GameCamera _camera;
    private Ship _ship;

    void Awake()
    {
        _instance = this;
        _scene = FindObjectOfType<SceneManager>();
        _camera = FindObjectOfType<GameCamera>();
        _ship = FindObjectOfType<Ship>();
    }

    void OnDestroy()
    {
        _instance = null;
    }
}
