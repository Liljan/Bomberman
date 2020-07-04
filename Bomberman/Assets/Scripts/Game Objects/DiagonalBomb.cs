using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagonalBomb : MonoBehaviour {

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
            LevelEvents.Instance().InvokeSpawnExplosionDiagonal(this.transform.position);

            this.gameObject.SetActive(false);
        }
	}
}
