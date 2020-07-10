using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthogonalBomb : MonoBehaviour {

    public float countdown = 2.0f;
    private float countdownTimer;

    private void OnEnable()
    {
        countdownTimer = countdown;
    }

    // Update is called once per frame
    void Update()
    {
        countdownTimer -= Time.deltaTime;

        if(countdownTimer <= 0.0f)
            Explode();
	}

    private void Explode()
    {
        LevelEvents.Instance().InvokeSpawnExplosionOrthogonal(this.transform.position);
        this.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Explosion")
            Explode();
    }
}
