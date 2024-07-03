using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float runSpeed = 8f;
    [SerializeField] float rotationSpeed = 500f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] private GameObject characterModel;

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
        //UpdateCharacterVisibility();
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

        if (movement.magnitude > 0.1f)
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
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        float moveAmount = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).magnitude / currentSpeed;
        animator.SetFloat("moveSpeed", moveAmount, 0.1f, Time.deltaTime);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isRunning", isRunning);
    }
    private void UpdateCharacterVisibility()
    {
        if (characterModel != null)
        {
            characterModel.SetActive(cameraController.cameraMode == CameraController.CameraMode.ThirdPerson);
        }
    }
}