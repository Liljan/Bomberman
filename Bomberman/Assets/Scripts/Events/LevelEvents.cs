using System;
using UnityEngine;

public class LevelEvents : MonoBehaviour {

    private static LevelEvents instance;

    // Level events
    public event Action<Vector3> SpawnOrthogonalBomb;
    public event Action<Vector3> SpawnDiagonalBomb;
    public event Action<Vector3> SpawnExplosionOrthogonal;
    public event Action<Vector3> SpawnExplosionDiagonal;

    private void Awake()
    {
        instance = this;
    }

    public static LevelEvents Instance()
    {
        return instance;
    }

    public void InvokeSpawnOrthogonalBomb(Vector3 pos)
    {
        SpawnOrthogonalBomb.Invoke(pos);
    }

    public void InvokeSpawnDiagonalBomb(Vector3 pos)
    {
        SpawnDiagonalBomb.Invoke(pos);
    }
    public void InvokeSpawnExplosionOrthogonal(Vector3 pos)
    {
        SpawnExplosionOrthogonal.Invoke(pos);
    }

    public void InvokeSpawnExplosionDiagonal(Vector3 pos)
    {
        SpawnExplosionDiagonal.Invoke(pos);
    }

}
