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

    private 

    // Update is called once per frame
    void Update () {
        countdownTimer -= Time.deltaTime;

        if(countdownTimer <= 0.0f)
            Explode();
	}

    private void Explode()
    {
        LevelEvents.Instance().InvokeSpawnExplosionDiagonal(this.transform.position);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Explosion")
            Explode();
    }
}
