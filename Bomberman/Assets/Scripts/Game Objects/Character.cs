using UnityEngine;

public class Character : MonoBehaviour
{
    private string m_HorizontalAxis;
    private string m_VerticalAxis;
    private string m_ActionButton;
    private string m_ActionButtonAlt;

    public int m_Id;
    public float m_Speed = 1.0f;

    public int m_BombsMax = 3;
    private int m_Bombs;
    public float m_BombRechargeTime = 3.0f;
    private float m_BombRechargeTimer;

    public int m_HealthMax = 3;
    private int m_Health;
    public float m_ImmunityTimeMax = 0.5f;
    private float m_ImmunityTime;

    private Rigidbody2D m_Rb2d;
    private Animator m_Animator;

    private void Awake()
    {
        m_Rb2d = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        m_Bombs = m_BombsMax;
        m_Health = m_HealthMax;
        m_ImmunityTime = m_ImmunityTimeMax;

        m_BombRechargeTimer = m_BombRechargeTime;
    }

    private void Start()
    {
        UIEvents.Instance().InvokeUpdateHealth(m_Id, m_Health);
        UIEvents.Instance().InvokeUpdateBomb(m_Id, m_Bombs);
    }

    private void MoveWithSticks()
    {
        float x = Input.GetAxis(m_HorizontalAxis);
        float y = Input.GetAxis(m_VerticalAxis);

        m_Rb2d.velocity = m_Speed * NormalizeVelocity(x, y);

        m_Animator.SetBool("Left", x > 0.0f);
        m_Animator.SetBool("Right", x < -0.0f);
        m_Animator.SetBool("Up", y > 0.0f);
        m_Animator.SetBool("Down", y < -0.0f);
    }

    private Vector3 NormalizeVelocity(float vx, float vy)
    {
        float magnitude = Mathf.Sqrt(vx * vx + vy * vy);
        
        // Replace with float compare
        if(magnitude == 0.0f)
            return Vector3.zero;

        return new Vector3(vx / magnitude, vy / magnitude, 0.0f);
    }


    private void RechargeBombs()
    {
        if(m_Bombs < m_BombsMax)
        {
            m_BombRechargeTimer -= Time.deltaTime;

            if(m_BombRechargeTimer <= 0.0f)
            {
                m_BombRechargeTimer = m_BombRechargeTime;
                m_Bombs++;
                UIEvents.Instance().InvokeUpdateBomb(m_Id, m_Bombs);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        //MoveStickSensitivity();
        MoveWithSticks();

        if (Input.GetButtonDown(m_ActionButton) && m_Bombs > 0)
            DropOrthogonalBomb();
        else if (Input.GetButtonDown(m_ActionButtonAlt))
            DropDiagonalBomb();            

        RechargeBombs();
    }


    public void SetID(int id)
    {
        this.m_Id = id;

        SetUpInput();
    }

    private void SetUpInput()
    {
        m_HorizontalAxis = InputMapping.LEFT_HORIZONTAL[m_Id - 1];
        m_VerticalAxis = InputMapping.LEFT_VERTICAL[m_Id - 1];
        m_ActionButton = InputMapping.ACTION[m_Id - 1];

        m_ActionButtonAlt = InputMapping.ACTION_ALT[m_Id - 1];
    }


    private void TakeDamage(int damage)
    {
        m_Health -= damage;
        m_Health = Mathf.Max(0, m_Health);
        UIEvents.Instance().InvokeUpdateHealth(m_Id, m_Health);
    }

    private void DropOrthogonalBomb()
    {
        LevelEvents.Instance().InvokeTrySpawnOrthogonalBomb(transform.position, this);
    }

    private void DropDiagonalBomb()
    {
        LevelEvents.Instance().InvokeSpawnDiagonalBomb(transform.position, this);
    }

    public void CallbackDropOrthogonalBomb(bool result)
    {
        if(!result)
            return;

        m_Bombs--;
        UIEvents.Instance().InvokeUpdateBomb(m_Id, m_Bombs);
    }

    public void CallbackDropDiagonalBomb(bool result)
    {
        if(!result)
            return;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Explosion")
            TakeDamage(1);
    }


}
