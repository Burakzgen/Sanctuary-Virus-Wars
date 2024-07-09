using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float runSpeed = 8f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float rotationSpeed = 500f;

    [Header("Ground Check Settings")]
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;

    private bool _isGrounded;
    private float _ySpeed;
    private bool _isRunning;
    private CharacterController _characterController;
    private CameraController _cameraController;
    private Animator _animator;
    private PlayerHealth _playerHealth;

    private float originalWalkSpeed;
    private float originalRunSpeed;
    [SerializeField] private float poisonedWalkSpeed = 3f;
    [SerializeField] private float poisonedRunSpeed = 5f;
    private bool isPoisoned = false;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _cameraController = Camera.main.GetComponent<CameraController>();
        _animator = GetComponentInChildren<Animator>();
        _playerHealth = GetComponent<PlayerHealth>();

        originalWalkSpeed = walkSpeed;
        originalRunSpeed = runSpeed;
    }

    private void Update()
    {
        GroundCheck();
        HandleMovement();
        HandleJump();
        UpdateAnimator();
    }

    void GroundCheck()
    {
        _isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }

    void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        _isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = _isRunning ? runSpeed : walkSpeed;

        // Zehirlenme durumu kontrolü
        if (isPoisoned)
        {
            currentSpeed = _isRunning ? poisonedRunSpeed : poisonedWalkSpeed;
        }

        Vector3 movement = _cameraController.PlanarRotation * new Vector3(horizontalInput, 0, verticalInput).normalized;

        if (_cameraController.cameraMode == CameraController.CameraMode.FirstPerson)
        {
            transform.rotation = Quaternion.Euler(0, _cameraController.transform.eulerAngles.y, 0);
        }
        else if (movement.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (_isGrounded && _ySpeed < 0)
        {
            _ySpeed = -0.5f;
        }
        else
        {
            _ySpeed += Physics.gravity.y * Time.deltaTime;
        }

        movement.y = _ySpeed;
        _characterController.Move(movement * currentSpeed * Time.deltaTime);
    }

    void HandleJump()
    {
        if (_isGrounded && Input.GetButtonDown("Jump"))
        {
            _ySpeed = jumpForce;
        }
    }

    void UpdateAnimator()
    {
        if (_animator != null)
        {
            float currentSpeed = _isRunning ? runSpeed : walkSpeed;
            if (isPoisoned)
            {
                currentSpeed = _isRunning ? poisonedRunSpeed : poisonedWalkSpeed;
            }

            float moveAmount = new Vector3(_characterController.velocity.x, 0, _characterController.velocity.z).magnitude / currentSpeed;
            _animator.SetFloat("moveSpeed", moveAmount, 0.05f, Time.deltaTime);
            _animator.SetBool("isGrounded", _isGrounded);
            _animator.SetBool("isRunning", _isRunning);
        }
    }

    public void SetPoisonedState(bool poisoned)
    {
        isPoisoned = poisoned;
        if (!poisoned)
        {
            walkSpeed = originalWalkSpeed;
            runSpeed = originalRunSpeed;
        }
    }
}
