using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExplosionHandler : MonoBehaviour
{
    [Header("Level Tilemap")]

    [SerializeField] private TilemapHandler m_TilemapHandler;

    [Header("Spawnable Gameobject Prefabs")]

    [SerializeField] private GameObject m_OrthogonalBomb;
    [SerializeField] private GameObject m_DiagonalBomb;
    [SerializeField] private GameObject m_Explosion;

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




    public void TrySpawnOrthogonalBomb(Vector3 pos, Character player = null)
    {
        Vector3Int cell = m_TilemapHandler.WorldToCell(pos);

        if(!m_TilemapHandler.IsCellSpawnable(cell))
        {
            player?.CallbackDropOrthogonalBomb(false);
            return;
        }

        Vector3 cellCenterPosition = m_TilemapHandler.CellCenterToWorld(cell);

        bool result = ObjectPoolManager.Instance().Spawn(m_OrthogonalBomb.GetInstanceID(), cellCenterPosition, Quaternion.identity);
        player?.CallbackDropOrthogonalBomb(result);
    }

    public void TrySpawnDiagonalBomb(Vector3 pos, Character player = null)
    {
        Vector3Int cell = m_TilemapHandler.WorldToCell(pos);

        if(!m_TilemapHandler.IsCellSpawnable(cell))
        {
            player?.CallbackDropOrthogonalBomb(false);
            return;
        }

        Vector3 cellCenterPosition = m_TilemapHandler.CellCenterToWorld(cell);

        bool result = ObjectPoolManager.Instance().Spawn(m_DiagonalBomb.GetInstanceID(), cellCenterPosition, Quaternion.identity);
        player?.CallbackDropDiagonalBomb(result);
    }


    public void SpawnExplosionOrthogonal(Vector3 pos)
    {
        Vector3Int cellPos = m_TilemapHandler.WorldToCell(pos);

        ExplodeCell(cellPos, Vector3Int.zero);

        ExplodeCell(cellPos + Vector3Int.up, Vector3Int.up);
        ExplodeCell(cellPos + Vector3Int.down, Vector3Int.down);
        ExplodeCell(cellPos + Vector3Int.left, Vector3Int.left);
        ExplodeCell(cellPos + Vector3Int.right, Vector3Int.right);
    }

    public void SpawnExplosionDiagonal(Vector3 pos)
    {
        Vector3Int cellPos = m_TilemapHandler.WorldToCell(pos);

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
        Vector3 cellCenterPosition = m_TilemapHandler.CellCenterToWorld(pos);

        if(m_TilemapHandler.IsWall(pos))
            return;

        if(m_TilemapHandler.IsDestructable(pos))
        {
            StartCoroutine(SpawnExplosionDelay(pos, dir));
            m_TilemapHandler.RemoveTile(pos);
            return;
        }

        StartCoroutine(SpawnExplosionDelay(pos, dir));
    }

    private IEnumerator SpawnExplosionDelay(Vector3Int position, Vector3Int direction)
    {
        Vector3 cellCenterPosition = m_TilemapHandler.CellCenterToWorld(position);
        ObjectPoolManager.Instance().Spawn(m_Explosion.GetInstanceID(), cellCenterPosition, Quaternion.identity);
        yield return new WaitForSeconds(0.01f);

        if(direction != Vector3Int.zero)
            ExplodeCell(position + direction, direction);
    }
}
