using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombSpawner : MonoBehaviour
{
    public Camera cam;
    public Tilemap tileMap;

    public ObjectPool bombPool;
    public ObjectPool explosionPool;

    public TileBase destructableTile;
    public TileBase wallTile;

    // Use this for initialization
    void Start()
    {
        LevelEvents.Instance().ExplodeBomb += SpawnExplosion;
        LevelEvents.Instance().SpawnBomb += SpawnBomb;
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    void SpawnBomb(Vector3 pos)
    {
        Vector3Int cell = tileMap.WorldToCell(pos);
        Vector3 cellCenterPosition = tileMap.GetCellCenterWorld(cell);

        bombPool.SpawnObject(cellCenterPosition, Quaternion.identity);
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
        TileBase tile = tileMap.GetTile<TileBase>(pos);
        Vector3 cellCenterPosition = tileMap.GetCellCenterWorld(pos);

        if (tile == wallTile)
        {
            return;
        }
            
        if (tile == destructableTile)
        {  
            explosionPool.SpawnObject(cellCenterPosition, Quaternion.identity);
            tileMap.SetTile(pos, null);
            return;
        }

        // else
        explosionPool.SpawnObject(cellCenterPosition, Quaternion.identity);

        ExplodeCell(pos + dir, dir);
    }
}
