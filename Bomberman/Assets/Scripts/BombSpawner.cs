using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombSpawner : MonoBehaviour
{
    public Camera camera;
    public Tilemap tileMap;
    public ObjectPool bombPool;

    private void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = camera.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0.0f;

            bombPool.SpawnObject(worldPos, Quaternion.identity);

        }
    }
}
