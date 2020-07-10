using UnityEngine;

public class Character : MonoBehaviour
{
    private string m_HorizontalAxis;
    private string m_VerticalAxis;
    private string m_ActionButton;

    public int m_Id;
    public float m_Speed = 1.0f;

    public int m_BombsMax = 3;
    private int m_Bombs;
    public float m_BombRechargeTime = 3.0f;
    private float m_BombRechargeTimer;

    public int m_HealthMax = 3;
    private int m_Health;

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

        m_Rb2d.velocity = new Vector3(x * m_Speed, y * m_Speed, 0.0f);

        m_Animator.SetBool("Left", x > 0.05f);
        m_Animator.SetBool("Right", x < -0.05f);
        m_Animator.SetBool("Up", y > 0.05f);
        m_Animator.SetBool("Down", y < -0.05f);
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

        if(Input.GetButtonDown(m_ActionButton) && m_Bombs > 0)
        {
            TryDropOrthogonalBomb();
        }

        RechargeBombs();
    }


    public void SetID(int ID)
    {
        this.m_Id = ID;

        m_HorizontalAxis = InputMapping.LEFT_HORIZONTAL[ID - 1];
        m_VerticalAxis = InputMapping.LEFT_VERTICAL[ID - 1];
        m_ActionButton = InputMapping.ACTION[ID - 1];
    }


    private void TakeDamage(int damage)
    {
        m_Health -= damage;
        m_Health = Mathf.Max(0, m_Health);
        UIEvents.Instance().InvokeUpdateHealth(m_Id, m_Health);
    }


    private void TryDropOrthogonalBomb()
    {
        LevelEvents.Instance().InvokeTrySpawnOrthogonalBomb(transform.position, this);
    }

    public void CallbackDropOrthogonalBomb(bool result)
    {
        if(!result)
            return;

        m_Bombs--;
        UIEvents.Instance().InvokeUpdateBomb(m_Id, m_Bombs);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Explosion")
            TakeDamage(1);
    }


}
