using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class ExplosionHandler : MonoBehaviour
{
    public Tilemap tileMap;

    public ObjectPool bombPool;
    public ObjectPool explosionPool;

    public TileBase destructableTile;
    public TileBase wallTile;


    public void SpawnBomb(Vector3 pos)
    {
        Vector3Int cell = tileMap.WorldToCell(pos);
        Vector3 cellCenterPosition = tileMap.GetCellCenterWorld(cell);

        bombPool.SpawnObject(cellCenterPosition, Quaternion.identity);
    }

    public void SpawnExplosionOrthogonal(Vector3 pos)
    {
        Vector3Int cellPos = tileMap.WorldToCell(pos);

        ExplodeCell(cellPos, Vector3Int.up);
        ExplodeCell(cellPos, Vector3Int.down);
        ExplodeCell(cellPos, Vector3Int.left);
        ExplodeCell(cellPos, Vector3Int.right);
    }

    public void SpawnExplosionDiagonal(Vector3 pos)
    {
        Vector3Int cellPos = tileMap.WorldToCell(pos);

        ExplodeCell(cellPos, new Vector3Int(1, 1, 0));
        ExplodeCell(cellPos, new Vector3Int(-1, 1, 0));
        ExplodeCell(cellPos, new Vector3Int(1, -1, 0));
        ExplodeCell(cellPos, new Vector3Int(-1, -1, 0));
    }

    private void ExplodeCell(Vector3Int pos, Vector3Int dir)
    {
        TileBase tile = tileMap.GetTile<TileBase>(pos);
        Vector3 cellCenterPosition = tileMap.GetCellCenterWorld(pos);

        if (tile == wallTile)
            return;

        if (tile == destructableTile)
        {
            //explosionPool.SpawnObject(cellCenterPosition, Quaternion.identity);
            tileMap.SetTile(pos, null);
            return;
        }

        // else
        StartCoroutine(SpawnExplosionDelay(pos + dir, dir));
    }

    private IEnumerator SpawnExplosionDelay(Vector3Int position, Vector3Int direction)
    {
        yield return new WaitForSeconds(0.01f);
        explosionPool.SpawnObject(position, Quaternion.identity);
        ExplodeCell(position, direction);
    }
}
