using System;
using UnityEngine;

public class LevelEvents : MonoBehaviour {

    private static LevelEvents instance;
    // Level events
    public event Action<Vector3> SpawnBomb;
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

    public void InvokeSpawnBomb(Vector3 pos)
    {
        SpawnBomb.Invoke(pos);
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
