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

    private bool isGrounded;
    private float ySpeed;
    private bool isRunning;
    private CharacterController characterController;
    private CameraController cameraController;
    private Animator animator;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        GroundCheck();
        HandleMovement();
        HandleJump();
        UpdateAnimator();
        HandleAttack();
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }

    void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        Vector3 movement = cameraController.PlanarRotation * new Vector3(horizontalInput, 0, verticalInput).normalized;

        if (cameraController.cameraMode == CameraController.CameraMode.FirstPerson)
        {
            transform.rotation = Quaternion.Euler(0, cameraController.transform.eulerAngles.y, 0);
        }
        else if (movement.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (isGrounded && ySpeed < 0)
        {
            ySpeed = -0.5f;
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }

        movement.y = ySpeed;
        characterController.Move(movement * currentSpeed * Time.deltaTime);
    }

    void HandleJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            ySpeed = jumpForce;
        }
    }

    void UpdateAnimator()
    {
        if (animator != null)
        {
            float currentSpeed = isRunning ? runSpeed : walkSpeed;
            float moveAmount = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).magnitude / currentSpeed;
            animator.SetFloat("moveSpeed", moveAmount, 0.05f, Time.deltaTime);
            animator.SetBool("isGrounded", isGrounded);
            animator.SetBool("isRunning", isRunning);
        }
    }

    void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("attack");
        }
    }
}
