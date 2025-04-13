using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    private bool isGrounded;
    private bool isDead;

    private ParticleSystem _particleSystem;
    private TrailRenderer trailRenderer;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    //Movement
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldownTime;

    public bool canDash = false;
    public bool canMove = false;

    private float moveDirection;
    private bool isDashing;

    private int touchingJumpTileCount;

    //Input
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dashAction;
    private InputAction pauseAction;

    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip deathSound;

    private PauseMenuManager pauseMenuManager;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
        playerInput = GetComponent<PlayerInput>();
        _particleSystem = GetComponent<ParticleSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        dashAction = playerInput.actions["Dash"];
        pauseAction = playerInput.actions["Pause"];
    }

    private void Start()
    {
        pauseMenuManager = FindObjectOfType<PauseMenuManager>();
        transform.position = spawnPoint.position;
    }

    private void Update()
    {
        Move();
    }

    private void OnEnable()
    {
        moveAction.performed += StartMove;
        moveAction.canceled += EndMove;
        dashAction.performed += Dash;
        jumpAction.performed += Jump;
        pauseAction.performed += TogglePause;
    }

    private void OnDisable()
    {
        moveAction.performed -= StartMove;
        moveAction.canceled -= EndMove;
        dashAction.performed -= Dash;
        jumpAction.performed -= Jump;
        pauseAction.performed -= TogglePause;
    }

    #region Movement

    private void StartMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<float>();
    }

    private void Move()
    {
        if (isDashing || moveDirection == 0 || isDead || !canMove || pauseMenuManager.IsPaused()) { return; }
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
    }

    private void EndMove(InputAction.CallbackContext context)
    {
        moveDirection = 0;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded && !isDead && canMove && !pauseMenuManager.IsPaused())
        {
            AudioSystem.instance.PlaySoundAtLocation(jumpSound, transform.position);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            LoadManager.Instance.Data.Stats.Jumps++;
        }
    }

    private void ApplyDash()
    {
        if (!isDashing) { return; }

        AudioSystem.instance.PlaySoundAtLocation(dashSound, transform.position);
        rb.AddForce(new Vector2(moveDirection, 0) * dashForce, ForceMode2D.Impulse);
        LoadManager.Instance.Data.Stats.Dashes++;
        StartCoroutine(StartDashCooldown());
    }

    private void Dash(InputAction.CallbackContext context)
    {
        if (moveDirection == 0 || !canDash || isDashing || pauseMenuManager.IsPaused()) { return; }

        StartCoroutine(DashController());
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Hazard") ||
            col.gameObject.layer == LayerMask.NameToLayer("NonCollisionHazard"))
        {
            if(!isDead)
                StartCoroutine(Kill());
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("Ground") || 
            col.gameObject.layer == LayerMask.NameToLayer("NonCollisionGround"))
        {
            touchingJumpTileCount += 1;

            if (touchingJumpTileCount > 0)
            {
                isGrounded = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Hazard") ||
            col.gameObject.layer == LayerMask.NameToLayer("NonCollisionHazard"))
        {
            if(!isDead)
                StartCoroutine(Kill());
        }
    }

    public void ChangeColour()
    {
        GetComponent<SpriteRenderer>().color = Color.magenta;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            col.gameObject.layer == LayerMask.NameToLayer("NonCollisionGround"))
        {
            touchingJumpTileCount -= 1;

            if (touchingJumpTileCount <= 0)
            {
                isGrounded = false;
            }
        }
    }

    public void ChangeSpawnPoint(Vector2 pos)
    {
        spawnPoint.position = pos;
    }

    public IEnumerator Kill()
    {
        AudioSystem.instance.PlaySoundAtLocation(deathSound, transform.position);
        UnityEngine.Camera.main.GetComponent<CameraShake>().StartShake(0.2f);
        isDead = true;
        LoadManager.Instance.Data.Stats.Deaths++;
        _particleSystem.Play();
        spriteRenderer.color = Color.red;
        yield return new WaitWhile(() => _particleSystem.isPlaying);
        spriteRenderer.color = Color.white;
        transform.position = spawnPoint.position;
        trailRenderer.Clear();
        rb.velocity = Vector2.zero;
        isDead = false;
    }

    private IEnumerator DashController()
    {
        isDashing = true;
        ApplyDash();
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }

    private IEnumerator StartDashCooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldownTime);
        canDash = true;
    }

    void TogglePause(InputAction.CallbackContext context)
    {
        if(!pauseMenuManager) {return;}

        if (pauseMenuManager.IsPaused())
        {
            pauseMenuManager.Resume();
        }
        else
        {
            pauseMenuManager.Pause();
        }
    }
}
