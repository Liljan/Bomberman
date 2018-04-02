using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombSpawner : MonoBehaviour
{
    public Camera camera;
    public Tilemap tileMap;

    public ObjectPool bombPool;
    public ObjectPool explosionPool;

    public RuleTile destructableTile;
    public TileBase wallTile;

    private void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        LevelEvents.Instance().ExplodeBomb += SpawnExplosion;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = camera.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int cell = tileMap.WorldToCell(worldPos);
            Vector3 cellCenterPosition = tileMap.GetCellCenterWorld(cell);

            bombPool.SpawnObject(cellCenterPosition, Quaternion.identity);
        }
    }

    void SpawnExplosion(Vector3 pos)
    {
        Vector3Int cellPos = tileMap.WorldToCell(pos);

        ExplodeCell(cellPos, Vector3Int.up);
        ExplodeCell(cellPos, Vector3Int.down);
        ExplodeCell(cellPos, Vector3Int.left);
        ExplodeCell(cellPos, Vector3Int.right);
    }

    void ExplodeCell(Vector3Int pos, Vector3Int dir)
    {
        Tile tile = tileMap.GetTile<Tile>(pos);

        if (tile == wallTile)
        {
            return;
        }
            
        if (tile == destructableTile)
        {
            tileMap.SetTile(pos, null);
            Vector3 cellCenterPosition = tileMap.GetCellCenterWorld(pos);
            explosionPool.SpawnObject(cellCenterPosition, Quaternion.identity);
            return;
        }

        // else
        ExplodeCell(pos + dir, dir);
    }
}
