﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public float countdown = 2.0f;
    private float countdownTimer;

    private void OnEnable()
    {
        countdownTimer = countdown;
    }

    // Update is called once per frame
    void Update () {
        countdownTimer -= Time.deltaTime;

        if(countdownTimer <= 0.0f)
        {
            LevelEvents.Instance().InvokeExplodeBomb(transform.position);
            this.gameObject.SetActive(false);
        }
	}
}
