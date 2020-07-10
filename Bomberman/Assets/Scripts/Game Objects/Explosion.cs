using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float time = 1.0f;
    private float countdownTimer;

    public void Awake()
    {
        countdownTimer = time;
    }

    // Use this for initialization
    void OnEnable()
    {
        countdownTimer = time;
    }

    // Update is called once per frame
    void Update()
    {
        countdownTimer -= Time.deltaTime;

        if(countdownTimer <= 0.0f)
        {
            this.gameObject.SetActive(false);
        }
    }
}
