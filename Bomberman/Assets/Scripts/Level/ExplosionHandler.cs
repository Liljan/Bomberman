using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class ExplosionHandler : MonoBehaviour
{
    public Tilemap _tileMap;
    public Tilemap _gameArea;

    public ObjectPool _orthogonalBombPool;
    public ObjectPool _diagonalBombPool;
    public ObjectPool _explosionPool;

    public TileBase _destructableTile;
    public TileBase _wallTile;

    public LayerMask _ObjectLayers;

    private void Awake()
    {
    }

    private void OnEnable()
    {
        LevelEvents.Instance().SpawnExplosionOrthogonal += SpawnExplosionOrthogonal;
        LevelEvents.Instance().SpawnExplosionDiagonal += SpawnExplosionDiagonal;

        LevelEvents.Instance().TrySpawnOrthogonalBomb += TrySpawnOrthogonalBomb;
        LevelEvents.Instance().SpawnDiagonalBomb += SpawnDiagonalBomb;
    }

    private void OnDisable()
    {
        LevelEvents.Instance().SpawnExplosionOrthogonal -= SpawnExplosionOrthogonal;
        LevelEvents.Instance().SpawnExplosionDiagonal -= SpawnExplosionDiagonal;

        LevelEvents.Instance().TrySpawnOrthogonalBomb -= TrySpawnOrthogonalBomb;
        LevelEvents.Instance().SpawnDiagonalBomb -= SpawnDiagonalBomb;
    }

    private bool IsInsideGameArea(Vector3Int point)
    {
        return _gameArea.GetTile<TileBase>(point) != null;
    }


    public void TrySpawnOrthogonalBomb(Vector3 pos, Character player = null)
    {
        Vector3Int cell = _tileMap.WorldToCell(pos);

        if(!IsInsideGameArea(cell) || !IsTileEmpty(cell))
        {
            player?.CallbackDropOrthogonalBomb(false);
            return;
        } 

        Vector3 cellCenterPosition = _tileMap.GetCellCenterWorld(cell);
        bool result = _orthogonalBombPool.SpawnObject(cellCenterPosition, Quaternion.identity);
        player?.CallbackDropOrthogonalBomb(result);
    }

    public void SpawnDiagonalBomb(Vector3 pos)
    {
        Vector3Int cell = _tileMap.WorldToCell(pos);
        Vector3 cellCenterPosition = _tileMap.GetCellCenterWorld(cell);

        _diagonalBombPool.SpawnObject(cellCenterPosition, Quaternion.identity);
    }


    public void SpawnExplosionOrthogonal(Vector3 pos)
    {
        Vector3Int cellPos = _tileMap.WorldToCell(pos);

        ExplodeCell(cellPos, Vector3Int.zero);

        ExplodeCell(cellPos + Vector3Int.up, Vector3Int.up);
        ExplodeCell(cellPos + Vector3Int.down, Vector3Int.down);
        ExplodeCell(cellPos + Vector3Int.left, Vector3Int.left);
        ExplodeCell(cellPos + Vector3Int.right, Vector3Int.right);
    }

    public void SpawnExplosionDiagonal(Vector3 pos)
    {
        Vector3Int cellPos = _tileMap.WorldToCell(pos);

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
        TileBase tile = _tileMap.GetTile<TileBase>(pos);
        Vector3 cellCenterPosition = _tileMap.GetCellCenterWorld(pos);

        if (tile == _wallTile)
            return;

        if (tile == _destructableTile)
        {
            _explosionPool.SpawnObject(cellCenterPosition, Quaternion.identity);
            _tileMap.SetTile(pos, null);
            return;
        }

        // If there is no propagation direction:
        // Spawn one explosion here with no propagation
        if(dir == Vector3Int.zero)
        {
            _explosionPool.SpawnObject(cellCenterPosition, Quaternion.identity);
            return;
        }  

        // else
        StartCoroutine(SpawnExplosionDelay(pos, dir));
    }

    private IEnumerator SpawnExplosionDelay(Vector3Int position, Vector3Int direction)
    {  
        Vector3 cellCenterPosition = _tileMap.GetCellCenterWorld(position);
        _explosionPool.SpawnObject(cellCenterPosition, Quaternion.identity);
        yield return new WaitForSeconds(0.01f);
        ExplodeCell(position + direction, direction);
    }


    private bool IsTileEmpty(Vector3Int cell)
    {
        TileBase tile = _tileMap.GetTile<TileBase>(cell);

        if(tile == _destructableTile || tile == _wallTile)
            return false;

        var tileBounds = _tileMap.GetBoundsLocal(cell);
        Vector3 position = _tileMap.GetCellCenterWorld(cell);
        return !Physics2D.OverlapBox(position, 0.5f * tileBounds.size, 0.0f, _ObjectLayers);
    }
}
