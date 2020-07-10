﻿using System;
using UnityEngine;

public class LevelEvents : MonoBehaviour {

    private static LevelEvents sm_Instance;

    // Level events
    public event Action<Vector3, Character> TrySpawnOrthogonalBomb;
    public event Action<Vector3> SpawnDiagonalBomb;
    public event Action<Vector3> SpawnExplosionOrthogonal;
    public event Action<Vector3> SpawnExplosionDiagonal;

    private void Awake()
    {
        sm_Instance = this;
    }

    public static LevelEvents Instance()
    {
        return sm_Instance;
    }

    public void InvokeTrySpawnOrthogonalBomb(Vector3 pos, Character caller = null)
    {
        TrySpawnOrthogonalBomb.Invoke(pos, caller);
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
