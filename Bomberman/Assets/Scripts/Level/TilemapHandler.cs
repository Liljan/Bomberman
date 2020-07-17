using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.Tilemaps;

public class TilemapHandler : MonoBehaviour
{
    public Tilemap m_Tilemap;
    private Dictionary<Vector3Int, TileBase> m_OriginalTiles;
    public Tilemap m_GameArea;

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
                }
    }

    public void ResetTilemap()
    {
        foreach(KeyValuePair<Vector3Int, TileBase> tile in m_OriginalTiles)
            m_Tilemap.SetTile(tile.Key, tile.Value);
    }

    public Vector3Int WorldToCell(Vector3 worldCoordinates)
    {
        return m_Tilemap.WorldToCell(worldCoordinates);
    }

    public Vector3 CellCenterToWorld(Vector3Int cellCoordinates)
    {
        return m_Tilemap.GetCellCenterWorld(cellCoordinates);
    }

    public bool IsCellSpawnable(Vector3Int cell)
    {
        return IsInsideGameArea(cell) && IsTileEmpty(cell);
    }

    public bool IsInsideGameArea(Vector3Int point)
    {
        return m_GameArea.GetTile<TileBase>(point) != null;
    }

    public bool IsWall(Vector3Int position)
    {
        TileBase tile = m_Tilemap.GetTile<TileBase>(position);
        return IsWall(tile);
    }

    public bool IsWall(TileBase t) { return t == m_WallTile; }

    public bool IsDestructable(Vector3Int position)
    {
        TileBase tile = m_Tilemap.GetTile<TileBase>(position);
        return IsDestructable(tile);
    }

    public bool IsDestructable(TileBase t) { return t == m_DestructableTile; }

    public void RemoveTile(Vector3Int position)
    {
        m_Tilemap.SetTile(position, null);
    }

    public bool IsTileEmpty(Vector3Int cell)
    {
        TileBase tile = m_Tilemap.GetTile<TileBase>(cell);

        if(IsDestructable(tile) || IsWall(tile))
            return false;

        var tileBounds = m_Tilemap.GetBoundsLocal(cell);
        Vector3 position = m_Tilemap.GetCellCenterWorld(cell);
        return !Physics2D.OverlapBox(position, 0.5f * tileBounds.size, 0.0f, m_ObjectLayers);
    }
}
