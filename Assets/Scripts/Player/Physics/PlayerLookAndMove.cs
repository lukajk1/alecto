using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLookAndMove : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Camera mainCamera; 

    [SerializeField] private InputSystem_Actions actions;

    [SerializeField] private Rigidbody rb; 
    [SerializeField] private LayerMask groundLayer;

    private Transform lastJumpedFrom;

    private float _moveSpeed;
    public float MoveSpeed
    {
        get => _moveSpeed;
        set
        {
            if (value > 0) 
            {
                _moveSpeed = value;
            }
        }
    }

    private float _jumpForce = 13f;
    public float JumpForce
    {
        get => _jumpForce;
        set
        {
            if (value > 0)
            {
                _jumpForce = value;
            }
        }
    }

    private float extraGravityForce = 19f;
    private float airStrafeInfluence = 0.75f; // modify currentSpeedMultiplier in the air instead?
    private float initJumpForce;
    private float initMoveSpeed;
    private float currentSpeedMultiplier = 1f;

    public float Sensitivity = 150f;
    private float xRotation;
    private float yRotation;

    private float coyoteTimeWindow = 0.3f;
    private float timeSinceGrounded = 0f;
    private bool hasJumped;

    private float slideDecayDuration = 1.2f;
    private float slideTimeElapsed = 0;
    private bool _isSliding;
    private float slideSpeedMultiplier = 1f;

    private bool _isGrounded;
    public bool IsGrounded
    {
        get => _isGrounded;
        private set
        {
            if (_isGrounded != value)
            {
                _isGrounded = value;
                OnGroundedChanged?.Invoke(_isGrounded);

                if (value) // switching from not grounded to grounded then
                {
                    timeSinceGrounded = 0f;
                    hasJumped = false;
                    SFXManager.i.PlaySFXClip(PlayerSFXList.i.jumpLanding, transform.position);
                }
            }
        }
    }

    public event Action<bool> OnGroundedChanged;

    private bool canCrouch = true;
    private float crouchCDLength = 0.5f;

    private bool _isCrouching;
    public bool IsCrouching
    {
        get => _isCrouching;
        set
        {
            if (_isCrouching != value)
            {
                _isCrouching = value;

                if (value && IsGrounded)
                {
                    SFXManager.i.PlaySFXClip(PlayerSFXList.i.slide, transform.position);
                }
                
                if (!value) // switching from false to true
                {
                    slideTimeElapsed = 0f;
                    slideSpeedMultiplier = 1f;
                }
            }
        }
    }

    private bool _isSlowfall;
    private bool IsSlowfall
    {
        get => _isSlowfall;
        set
        {
            if (_isSlowfall != value)
            {
                _isSlowfall = value;

                if (value)
                {
                    SFXManager.i.PlaySFXClip(PlayerSFXList.i.slowfall, transform.position);
                }
            }
        }
    }

    private Game game;
    [SerializeField] private PlayerUnit playerUnit;

    private float positionSaveInterval = 0.5f;
    private float timeSinceLastSave = 0f;

    private Vector3 crouchScale = new Vector3(1, 0.4f, 1);
    private Vector3 playerScale;

    private InputAction move;
    private InputAction jumpAction;
    private InputAction crouchAction;

    private Vector3 movementVector;
    private PlayerWallclimb playerWallClimb;
    private bool isFalling;

    private Timer timer;
    private void Awake()
    {
        initJumpForce = JumpForce; // these are here for resetting values after changing them with console commands
        initMoveSpeed = MoveSpeed;

        actions = new InputSystem_Actions(); 

        rb = player.GetComponent<Rigidbody>();
        _moveSpeed = playerUnit.BaseMoveSpeed;


        playerScale = player.transform.localScale;
        move = actions.Player.Move;
    }

    private void OnEnable()
    {
        actions.Player.Enable();

        actions.Player.Jump.performed += OnJumpPerformed;
        actions.Player.Crouch.performed += OnCrouchPerformed;
        actions.Player.Crouch.canceled += OnCrouchReleased;
    }

    private void OnDisable()
    {
        actions.Player.Jump.performed -= OnJumpPerformed;
        actions.Player.Crouch.performed -= OnCrouchPerformed;
        actions.Player.Crouch.canceled -= OnCrouchReleased;

        actions.Player.Disable();
    }
    private void Start()
    {
        game = Game.i;
        playerWallClimb = FindFirstObjectByType<PlayerWallclimb>();
        timer = new GameObject($"LookAndMove Timer").AddComponent<Timer>();
        lastJumpedFrom = new GameObject("lastJumpedFrom").transform;
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(movementVector.x, rb.linearVelocity.y, movementVector.z);

        timeSinceLastSave += Time.fixedDeltaTime;

        RaycastHit hit;
        IsGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, .35f);
        //Debug.DrawLine(transform.position, transform.position + Vector3.down * 0.35f, Color.red);

        rb.AddForce(Vector3.down * extraGravityForce, ForceMode.Force); // stick to ground better with a constant downwards force added

        if (rb.linearVelocity.y < -0.1f && !IsGrounded)
        {
            isFalling = true;
        }
        else isFalling = false;


        if (Input.GetKey(KeyCode.Space) && isFalling && !playerWallClimb.IsWallClimbing) // slow fall
        {
            IsSlowfall = true;
            rb.AddForce(Vector3.up * 28f, ForceMode.Force);
            //Debug.Log("should be slow falling");
        }

        if (IsSlowfall && !Input.GetKey(KeyCode.Space)) IsSlowfall = false;

        if (IsGrounded)
        {
            if (hit.collider.gameObject.layer == 4) // water
            {
                player.transform.position = lastJumpedFrom.position;
            }
            else
            {
                if (timeSinceLastSave > positionSaveInterval)
                {
                    lastJumpedFrom.position = rb.position;
                    timeSinceLastSave = 0f;
                }
            }

            if (!IsCrouching)
            {
                currentSpeedMultiplier = 1f;
            }
        }
        else
        {
            currentSpeedMultiplier = 1.4f;
            timeSinceGrounded += Time.fixedDeltaTime;
        }

        if (IsCrouching)
        {
            slideSpeedMultiplier = Mathf.Lerp(1.4f, 0.1f, slideTimeElapsed / slideDecayDuration);
            slideTimeElapsed += Time.fixedDeltaTime;
        }
        //Debug.Log(slideSpeedMultiplier);
    }

    private void Update()
    {
        if (!Game.IsPaused && !Game.IsInDialogue)
        {
            movementVector = DetermineMovementVector();

            DetermineCamMovement();
        }
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {

        if ((IsGrounded || timeSinceGrounded < coyoteTimeWindow) && !Game.IsPaused && !hasJumped)
        {
            Jump();
        }
    }

    public void Jump()
    {
        hasJumped = true;
        ForceMode forceMode = ForceMode.Impulse;

        player.transform.localScale = playerScale; // removes crouch mode
        IsCrouching = false;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // Reset vertical velocity
        rb.AddForce(Vector3.up * JumpForce, forceMode);
        SFXManager.i.PlaySFXClip(PlayerSFXList.i.jumpTakeoff, transform.position);
    }
    private void OnCrouchPerformed(InputAction.CallbackContext context)
    {
        if (!Game.IsPaused && canCrouch) 
        {
            player.transform.localScale = crouchScale;
            IsCrouching = true;
        }
    }
    private void OnCrouchReleased(InputAction.CallbackContext context)
    {
        if (!Game.IsPaused) 
        {
            player.transform.localScale = playerScale;
            IsCrouching = false;
        }

        CrouchCD();
    }

    private void DetermineCamMovement() // I guess this doesn't need to be broken out because where you're looking also determines how movement works?
    {
        xRotation -= Input.GetAxis("Mouse Y") * Time.deltaTime * Sensitivity;
        yRotation += Input.GetAxis("Mouse X") * Time.deltaTime * Sensitivity;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // prevent looking above/below 90

        mainCamera.transform.localEulerAngles = new Vector3(xRotation, yRotation, 0);
        transform.localEulerAngles = new Vector3(xRotation, yRotation, 0);
    }
    private Vector3 DetermineMovementVector()
    {
        Vector2 moveDir = move.ReadValue<Vector2>().normalized * MoveSpeed * currentSpeedMultiplier * (!IsGrounded ? airStrafeInfluence : 1) * slideSpeedMultiplier;

        // Get only the horizontal (yaw) rotation of the player, ignoring the pitch (up/down)
        float yRotation = transform.eulerAngles.y;

        // Calculate movement relative to the player's current horizontal rotation
        Vector3 forward = new Vector3(Mathf.Sin(yRotation * Mathf.Deg2Rad), 0, Mathf.Cos(yRotation * Mathf.Deg2Rad)) * moveDir.y;
        Vector3 right = transform.right * moveDir.x;

        Vector3 combined = forward + right;
        return new Vector3(combined.x, 0, combined.z);
    }

    private void CrouchCD()
    {
        canCrouch = false;
        StartCoroutine(timer.TimerCR(crouchCDLength, () => canCrouch = true));
    }
    public void ResetValues()
    {
        JumpForce = initJumpForce;
        MoveSpeed = initMoveSpeed;
    }

}
