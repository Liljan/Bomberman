using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float speed = 1.0f;

    public int bombAmount = 3;
    private int bombs;
    public float bombRechargeTime = 3.0f;
    private float bombRechargeTimer;

    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        bombs = bombAmount;
        bombRechargeTimer = bombRechargeTime;
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        rb2d.velocity = new Vector3(x * speed, y * speed, 0.0f);
    }

    private void RechargeBombs()
    {
        if (bombs < bombAmount)
        {
            bombRechargeTimer -= Time.deltaTime;

            if (bombRechargeTimer <= 0.0f)
            {
                bombRechargeTimer = bombRechargeTime;
                bombs++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if(Input.GetButtonDown("Bomb") && bombs > 0)
        {
            LevelEvents.Instance().InvokeSpawnBomb(transform.position);
            bombs--;
        }

        RechargeBombs();
    }
}
