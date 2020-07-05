using UnityEngine;

public class Character : MonoBehaviour
{
    private string HorizontalAxis;
    private string VerticalAxis;
    private string ActionButton;

    public int ID;
    public float speed = 1.0f;

    public int bombsMax = 3;
    private int _bombs;
    public float bombRechargeTime = 3.0f;
    private float _bombRechargeTimer;

    public int healthMax = 3;
    private int _health;

    private Rigidbody2D _rb2d;
    private Animator _animator;


    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }


    private void OnEnable()
    {
        _bombs = bombsMax;
        _health = healthMax;
        _bombRechargeTimer = bombRechargeTime;
    }


    private void Start()
    {
        UIEvents.Instance().InvokeUpdateHealth(ID, _health);
        UIEvents.Instance().InvokeUpdateBomb(ID, _bombs);
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
        if (_bombs < bombsMax)
        {
            _bombRechargeTimer -= Time.deltaTime;

            if (_bombRechargeTimer <= 0.0f)
            {
                _bombRechargeTimer = bombRechargeTime;
                _bombs++;
                UIEvents.Instance().InvokeUpdateBomb(ID, _bombs);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        //MoveStickSensitivity();
        MoveWithSticks();

        if (Input.GetButtonDown(ActionButton) && _bombs > 0)
        {
            DropBomb();
        }

        RechargeBombs();
    }


    public void SetID(int ID)
    {
        this.ID = ID;

        HorizontalAxis = InputMapping.LEFT_HORIZONTAL[ID - 1];
        VerticalAxis = InputMapping.LEFT_VERTICAL[ID - 1];
        ActionButton = InputMapping.ACTION[ID - 1];
    }


    private void TakeDamage(int damage)
    {
        _health -= damage;
        _health = Mathf.Max(0, _health);
        UIEvents.Instance().InvokeUpdateHealth(ID, _health);
    }


    private void DropBomb()
    {
        LevelEvents.Instance().InvokeSpawnOrthogonalBomb(transform.position);
        _bombs--;
        UIEvents.Instance().InvokeUpdateBomb(ID, _bombs);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Explosion")
            TakeDamage(1);
    }


}
