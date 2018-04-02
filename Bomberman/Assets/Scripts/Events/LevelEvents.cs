using System;
using UnityEngine;

public class LevelEvents : MonoBehaviour {

    private static LevelEvents instance;
    // Level events
    public event Action<Vector3> ExplodeBomb;

    private void Awake()
    {
        instance = this;
    }

    public static LevelEvents Instance()
    {
        return instance;
    }

    public void InvokeExplodeBomb(Vector3 pos)
    {
        ExplodeBomb.Invoke(pos);
    }

}
