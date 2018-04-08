using System;
using UnityEngine;

public class UIEvents : MonoBehaviour
{
    private static UIEvents instance;
    // Level events
    public event Action<int, int> UpdateHealth;
    public event Action<int, int> UpdateBombs;

    private void Awake()
    {
        instance = this;
    }

    public static UIEvents Instance()
    {
        return instance;
    }

    public void InvokeUpdateHealth(int ID, int health)
    {
        UpdateHealth.Invoke(ID, health);
    }

    public void InvokeUpdateBomb(int ID, int bombs)
    {
        UpdateBombs.Invoke(ID, bombs);
    }
}
