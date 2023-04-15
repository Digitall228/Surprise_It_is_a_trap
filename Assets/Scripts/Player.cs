using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICharacter
{
    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private ParticleSystem dustParticles;

    [Header("Walking")]
    [SerializeField] private float walkSpeed = 4;
    [SerializeField] private float acceleration = 2;
    [SerializeField] private float currentMovementLerpSpeed = 100;

    [Header("Sliding")]
    [SerializeField] private float slidingSpeed;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 15;
    [SerializeField] private float JumpDownVelocity = 8;
    [SerializeField] private float fallMultiplier = 7;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float groundPointOffset;
    [SerializeField] private Vector2 groundPointSize;
    [SerializeField] private float wallPointOffset;
    [SerializeField] private Vector2 wallPointSize;
    [SerializeField] private LayerMask groundMask;

    private bool isDead = false;
    private bool isGrounded;
    private float lastTimeGrounded;
    private bool isJumped = false;
    private float currentSpeed;

    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 leftScale = new Vector3(-1,1,1);
    private Vector2 rightScale = new Vector3(1,1,1);

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private readonly int JUMP = Animator.StringToHash("PlayerJump");
    void Start()
    {
        Time.timeScale = 1;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isDead)
        {
            float x = Input.GetAxis(HORIZONTAL);
            bool y = Input.GetKey(KeyCode.Space);

            HandleMoving(x);
            HandleJumping(y);
            HandleSliding();
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameController.instance.Pause();
            }
            else if (Input.GetKeyUp(KeyCode.R))
            {
                isDead = true;
                GameController.instance.GameOver();
            }
            if (transform.position.y < -10)
            {
                isDead = true;
                AudioController.instance.Sound(AudioType.TakeDamage);
                GameController.instance.GameOver();
            }
        }
        else
        {
            if (Input.anyKeyDown)
            {
                GameController.instance.Replay();
            }
        }
    }
    private void HandleMoving(float x)
    {
        var currentAcceleration = isGrounded ? acceleration : acceleration * 0.5f;

        if (Input.GetKey(KeyCode.A))
        {
            if (rb.velocity.x > 0) x = 0;
            x = Mathf.MoveTowards(x, -1, currentAcceleration * Time.deltaTime);

            if (transform.localScale.x != leftScale.x)
            {
                transform.localScale = leftScale;
                if (isGrounded)
                    dustParticles.Play();
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (rb.velocity.x < 0) x = 0;
            x = Mathf.MoveTowards(x, 1, currentAcceleration * Time.deltaTime);

            if (transform.localScale.x != rightScale.x)
            {
                transform.localScale = rightScale;
                if (isGrounded)
                    dustParticles.Play();
            }
        }
        else
        {
            x = Mathf.MoveTowards(x, 0, currentAcceleration * 2 * Time.deltaTime);
        }

        var newVelocity = new Vector3(x * walkSpeed, rb.velocity.y);
        rb.velocity = Vector3.MoveTowards(rb.velocity, newVelocity, currentMovementLerpSpeed * Time.deltaTime);
    }

    private void HandleJumping(bool isJumpPressed)
    {
        Collider2D[] colliders = new Collider2D[1];
        isGrounded = Physics2D.OverlapBoxNonAlloc(transform.position + new Vector3(0, groundPointOffset), groundPointSize, 0, colliders, groundMask) > 0;
        
        if(isGrounded && isJumped)
        {
            animator.SetBool("IsJumping", false);
            lastTimeGrounded = Time.time;
            isJumped = false;
            dustParticles.Play();
        }

        if (isJumpPressed && (isGrounded || (Time.time < lastTimeGrounded + coyoteTime && !isJumped)))
        {
            Jump(new Vector2(rb.velocity.x, jumpForce));
        }
        else if ((rb.velocity.y < JumpDownVelocity) && !isJumpPressed && rb.velocity.y > 0)
        {
            rb.velocity += fallMultiplier * Physics.gravity.y * Vector2.up * Time.deltaTime;
        }
    }
    private void HandleSliding()
    {
        Collider2D[] colliders = new Collider2D[1];
        bool isSliding = (Physics2D.OverlapBoxNonAlloc(transform.position + new Vector3(wallPointOffset, 0), wallPointSize, 0, colliders, groundMask) +
            Physics2D.OverlapBoxNonAlloc(transform.position - new Vector3(wallPointOffset, 0), wallPointSize, 0, colliders, groundMask)) > 0;
        if(isSliding && !isGrounded && rb.velocity.y <= 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, -Mathf.Clamp(rb.velocity.y, slidingSpeed, float.MaxValue));
        }
    }

    private void Jump(Vector2 direction)
    {
        isJumped = true;
        animator.SetBool("IsJumping", true);
        rb.velocity = direction;
    }
    public void Throw(float force)
    {
        rb.velocity = new Vector2(rb.velocity.x, force);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Obstacle"))
        {
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            AudioController.instance.Sound(AudioType.TakeDamage);
            isDead = true;
            GameController.instance.GameOver();
        }
        else if(collision.CompareTag("Coin"))
        {
            AudioController.instance.Sound(AudioType.CoinPickUp);
            collision.gameObject.SetActive(false);
            GameController.instance.AddCoin();
        }
        else if (collision.CompareTag("Finish"))
        {
            AudioController.instance.Sound(AudioType.Win);
            GameController.instance.Finish();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            AudioController.instance.Sound(AudioType.TakeDamage);
            isDead = true;
            GameController.instance.GameOver();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + new Vector3(0, groundPointOffset), groundPointSize);
        Gizmos.DrawWireCube(transform.position + new Vector3(wallPointOffset, 0), wallPointSize);
        Gizmos.DrawWireCube(transform.position - new Vector3(wallPointOffset, 0), wallPointSize);
    }
}
