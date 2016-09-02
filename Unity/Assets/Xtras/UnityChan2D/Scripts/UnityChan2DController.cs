using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class UnityChan2DController : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float jumpPower = 1000f;
    public Vector2 backwardForce = new Vector2(-4.5f, 5.4f);

    public LayerMask whatIsGround;

    private Animator m_animator;
    private CapsuleCollider2D m_CapsuleCollider2D;
    private Rigidbody2D m_rigidbody2D;
    private bool m_isGround;
    private const float m_centerY = 1.5f;

	public GameObject m_bullet;

    private State m_state = State.Normal;
	private CharacterDirection m_characterDirection = CharacterDirection.Right;
    void Reset()
    {
        Awake();

        // UnityChan2DController
        maxSpeed = 10f;
        jumpPower = 1000;
        backwardForce = new Vector2(-4.5f, 5.4f);
        //whatIsGround = 1 << LayerMask.NameToLayer("Cave");

        // Transform
        transform.localScale = new Vector3(1, 1, 1);

        // Rigidbody2D
        m_rigidbody2D.gravityScale = 3.5f;
        m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        // CapsuleCollider2D
        m_CapsuleCollider2D.size = new Vector2(1, 2.5f);
        m_CapsuleCollider2D.offset = new Vector2(0, -0.25f);

        // Animator
        m_animator.applyRootMotion = false;
    }

    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_CapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (m_state != State.Damaged)
        {
			float x = Input.GetAxis("Horizontal");
			float y = Input.GetAxis("Vertical");
            bool jump = Input.GetButtonDown("Jump");
			Move(x, y, jump);
        }
		bool shoot = Input.GetKeyDown(KeyCode.LeftControl);

		if (shoot) {
			Vector2 pos = transform.position;
			Vector3 startBulletPos = transform.position;
			Debug.Log (startBulletPos);
			startBulletPos.x += 30.0f;
			Debug.Log (startBulletPos);
			GameObject bullet = (GameObject)Instantiate (m_bullet, startBulletPos, transform.rotation);

			if (m_characterDirection == CharacterDirection.Left) {
				
				bullet.GetComponent<movement> ().direction = true;
			} else if (m_characterDirection == CharacterDirection.Right) {
				bullet.GetComponent<movement> ().direction = false;
			}
			//bullet.velocity = transform.forward * 1;

		}
    }

	void Move(float x, float y, bool jump)
    {

		if (x > 0) {
			m_characterDirection = CharacterDirection.Right;
		} else if (x < 0) {
			m_characterDirection = CharacterDirection.Left;
		}

		if (Mathf.Abs(x) > 0)
        {
            Quaternion rot = transform.rotation;
			transform.rotation = Quaternion.Euler(rot.x, Mathf.Sign(x) == 1 ? 0 : 180, rot.z);
        }

		m_rigidbody2D.velocity = new Vector2(x * maxSpeed, m_rigidbody2D.velocity.y);

		m_animator.SetFloat("Horizontal", x);
        m_animator.SetFloat("Vertical", m_rigidbody2D.velocity.y);
        m_animator.SetBool("isGround", m_isGround);

        if (jump && m_isGround)
        {
            m_animator.SetTrigger("Jump");
            SendMessage("Jump", SendMessageOptions.DontRequireReceiver);
            m_rigidbody2D.AddForce(Vector2.up * jumpPower);
        }


    }

    void FixedUpdate()
    {
        Vector2 pos = transform.position;
        Vector2 groundCheck = new Vector2(pos.x, pos.y - (m_centerY * transform.localScale.y));
        Vector2 groundArea = new Vector2(m_CapsuleCollider2D.size.x * 0.49f, 0.026f);

        m_isGround = Physics2D.OverlapArea(groundCheck + groundArea, groundCheck - groundArea, whatIsGround);
        m_animator.SetBool("isGround", m_isGround);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "DamageObject" && m_state == State.Normal)
        {
            m_state = State.Damaged;
            StartCoroutine(INTERNAL_OnDamage());
        }
    }

    IEnumerator INTERNAL_OnDamage()
    {
        m_animator.Play(m_isGround ? "Damage" : "AirDamage");
        m_animator.Play("Idle");

        SendMessage("OnDamage", SendMessageOptions.DontRequireReceiver);

        m_rigidbody2D.velocity = new Vector2(transform.right.x * backwardForce.x, transform.up.y * backwardForce.y);

        yield return new WaitForSeconds(.2f);

        while (m_isGround == false)
        {
            yield return new WaitForFixedUpdate();
        }
        m_animator.SetTrigger("Invincible Mode");
        m_state = State.Invincible;
    }

    void OnFinishedInvincibleMode()
    {
        m_state = State.Normal;
    }

    enum State
    {
        Normal,
        Damaged,
        Invincible
    }

	enum CharacterDirection
	{
		Left,
		Right,
		Up,
		Down
	}
}
