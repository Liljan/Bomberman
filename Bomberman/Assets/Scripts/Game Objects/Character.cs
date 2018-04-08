using UnityEngine;

public class Character : MonoBehaviour
{
    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";
    public float speed = 1.0f;

    public int bombAmount = 3;
    private int _bombs;
    public float bombRechargeTime = 3.0f;
    private float _bombRechargeTimer;

    private Rigidbody2D _rb2d;
    private Animator _animator;

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _bombs = bombAmount;
        _bombRechargeTimer = bombRechargeTime;
    }

    private void MoveWithSticks()
    {
        float x = Input.GetAxis(HorizontalAxis);
        float y = Input.GetAxis(VerticalAxis);

        _rb2d.velocity = new Vector3(x * speed, y * speed, 0.0f);

        _animator.SetBool("Left", x > 0.05f);
        _animator.SetBool("Right", x < -0.05f);
        _animator.SetBool("Up", y > 0.05f);
        _animator.SetBool("Down", y < -0.05f);
    }


    private void RechargeBombs()
    {
        if (_bombs < bombAmount)
        {
            _bombRechargeTimer -= Time.deltaTime;

            if (_bombRechargeTimer <= 0.0f)
            {
                _bombRechargeTimer = bombRechargeTime;
                _bombs++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //MoveStickSensitivity();
        MoveWithSticks();

        if (Input.GetButtonDown("Bomb") && _bombs > 0)
        {
            LevelEvents.Instance().InvokeSpawnBomb(transform.position);
            _bombs--;
        }

        RechargeBombs();
    }
}
