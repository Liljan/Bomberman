using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class ExplosionHandler : MonoBehaviour
{
    public Tilemap m_Tilemap;
    private Dictionary<Vector3Int, TileBase> m_OriginalTiles;
    public Tilemap m_GameArea;

    public GameObject m_OrthogonalBomb;
    public GameObject m_DiagonalBomb;
    public GameObject m_Explosion;

    public TileBase m_DestructableTile;
    public TileBase m_WallTile;

    public LayerMask m_ObjectLayers;

    private void Awake()
    {
        CopyTilemap();
    }

    private void CopyTilemap()
    {
        var bounds = m_Tilemap.cellBounds;
        int amount = bounds.size.x * bounds.size.y * bounds.size.z;

        m_OriginalTiles = new Dictionary<Vector3Int, TileBase>(amount);

        for(int x = bounds.min.x; x < bounds.max.x; x++)
            for(int y = bounds.min.y; y < bounds.max.y; y++)
                for(int z = bounds.min.z; z < bounds.max.z; z++)
                {
                    var pos = new Vector3Int(x, y, z);
                    var tile = m_Tilemap.GetTile(pos);
                    m_OriginalTiles.Add(pos, tile);

                    // test
                    m_Tilemap.SetTile(pos, null);
                }

        ResetTilemap();
    }

    private void ResetTilemap()
    {
        foreach(KeyValuePair<Vector3Int, TileBase> tile in m_OriginalTiles)
            m_Tilemap.SetTile(tile.Key, tile.Value);
    }

    private void OnEnable()
    {
        LevelEvents.Instance().SpawnExplosionOrthogonal += SpawnExplosionOrthogonal;
        LevelEvents.Instance().SpawnExplosionDiagonal += SpawnExplosionDiagonal;
        
        LevelEvents.Instance().SpawnOrthogonalBomb += TrySpawnOrthogonalBomb;
        LevelEvents.Instance().SpawnDiagonalBomb += TrySpawnDiagonalBomb;
    }

    private void OnDisable()
    {
        LevelEvents.Instance().SpawnExplosionOrthogonal -= SpawnExplosionOrthogonal;
        LevelEvents.Instance().SpawnExplosionDiagonal -= SpawnExplosionDiagonal;

        LevelEvents.Instance().SpawnOrthogonalBomb -= TrySpawnOrthogonalBomb;
        LevelEvents.Instance().SpawnDiagonalBomb -= TrySpawnDiagonalBomb;
    }

    private bool IsInsideGameArea(Vector3Int point)
    {
        return m_GameArea.GetTile<TileBase>(point) != null;
    }


    public void TrySpawnOrthogonalBomb(Vector3 pos, Character player = null)
    {
        Vector3Int cell = m_Tilemap.WorldToCell(pos);

        if(!IsInsideGameArea(cell) || !IsTileEmpty(cell))
        {
            player?.CallbackDropOrthogonalBomb(false);
            return;
        } 

        Vector3 cellCenterPosition = m_Tilemap.GetCellCenterWorld(cell);

        bool result = ObjectPoolManager.Instance().Spawn(m_OrthogonalBomb.GetInstanceID(), cellCenterPosition, Quaternion.identity);
        player?.CallbackDropOrthogonalBomb(result);
    }

    public void TrySpawnDiagonalBomb(Vector3 pos, Character player = null)
    {
        Vector3Int cell = m_Tilemap.WorldToCell(pos);

        if (!IsInsideGameArea(cell) || !IsTileEmpty(cell))
        {
            player?.CallbackDropOrthogonalBomb(false);
            return;
        }

        Vector3 cellCenterPosition = m_Tilemap.GetCellCenterWorld(cell);

        bool result = ObjectPoolManager.Instance().Spawn(m_DiagonalBomb.GetInstanceID(), cellCenterPosition, Quaternion.identity);
        player?.CallbackDropDiagonalBomb(result);
    }


    public void SpawnExplosionOrthogonal(Vector3 pos)
    {
        Vector3Int cellPos = m_Tilemap.WorldToCell(pos);

        ExplodeCell(cellPos, Vector3Int.zero);

        ExplodeCell(cellPos + Vector3Int.up, Vector3Int.up);
        ExplodeCell(cellPos + Vector3Int.down, Vector3Int.down);
        ExplodeCell(cellPos + Vector3Int.left, Vector3Int.left);
        ExplodeCell(cellPos + Vector3Int.right, Vector3Int.right);
    }

    public void SpawnExplosionDiagonal(Vector3 pos)
    {
        Vector3Int cellPos = m_Tilemap.WorldToCell(pos);

        ExplodeCell(cellPos, Vector3Int.zero);

        {
            Vector3Int dir = new Vector3Int(-1, -1, 0);
            ExplodeCell(cellPos + dir, dir);
        }

        {
            Vector3Int dir = new Vector3Int(1, -1, 0);
            ExplodeCell(cellPos + dir, dir);
        }

        {
            Vector3Int dir = new Vector3Int(-1, 1, 0);
            ExplodeCell(cellPos + dir, dir);
        }

        {
            Vector3Int dir = new Vector3Int(1, 1, 0);
            ExplodeCell(cellPos + dir, dir);
        }
    }

    private void ExplodeCell(Vector3Int pos, Vector3Int dir)
    {
        TileBase tile = m_Tilemap.GetTile<TileBase>(pos);
        Vector3 cellCenterPosition = m_Tilemap.GetCellCenterWorld(pos);

        if(IsWall(tile))
            return;

        if(IsDestructable(tile))
        {
            StartCoroutine(SpawnExplosionDelay(pos, dir));
            m_Tilemap.SetTile(pos, null);
            return;
        }

        StartCoroutine(SpawnExplosionDelay(pos, dir));
    }

    private IEnumerator SpawnExplosionDelay(Vector3Int position, Vector3Int direction)
    {  
        Vector3 cellCenterPosition = m_Tilemap.GetCellCenterWorld(position);
        ObjectPoolManager.Instance().Spawn(m_Explosion.GetInstanceID(), cellCenterPosition, Quaternion.identity);
        yield return new WaitForSeconds(0.01f);

        if(direction != Vector3Int.zero)
            ExplodeCell(position + direction, direction);
    }

    private bool IsWall(TileBase t) { return t == m_WallTile; }
    private bool IsDestructable(TileBase t) { return t == m_DestructableTile; }

    private bool IsTileEmpty(Vector3Int cell)
    {
        TileBase tile = m_Tilemap.GetTile<TileBase>(cell);
        
        if(IsDestructable(tile) || IsWall(tile))
            return false;

        var tileBounds = m_Tilemap.GetBoundsLocal(cell);
        Vector3 position = m_Tilemap.GetCellCenterWorld(cell);
        return !Physics2D.OverlapBox(position, 0.5f * tileBounds.size, 0.0f, m_ObjectLayers);
    }
}
