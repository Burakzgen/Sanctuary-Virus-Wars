using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    // Private
    Rigidbody _rg;
    private bool isPoisoned = false;
    private float originalWalkSpeed;
    private float originalRunSpeed;

    [SerializeField] float speed = 5;

    [Header("Running")]
    [SerializeField] bool canRun = true;
    [SerializeField] float runSpeed = 9;
    [SerializeField] KeyCode runningKey = KeyCode.LeftShift;

    [Header("Poisoned")]
    [SerializeField] private float poisonedWalkSpeed = 3f;
    [SerializeField] private float poisonedRunSpeed = 5f;

    [Header("Footstep")]
    private float footstepTimer = 0f;
    [SerializeField] private float baseFootstepInterval = 0.5f;
    [SerializeField] private float minFootstepInterval = 0.1f;
    private float currentFootstepInterval;
    [SerializeField] private FirstPersonGroundCheck groundCheck;
    [SerializeField] private FirstPersonJump jumpComponent;
    public bool IsPause = false;

    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();
    public bool IsRunning { get; private set; }

    void Awake()
    {
        _rg = GetComponent<Rigidbody>();

        originalWalkSpeed = speed;
        originalRunSpeed = runSpeed;
    }

    void FixedUpdate()
    {
        if (IsPause) return;
        IsRunning = canRun && Input.GetKey(runningKey);

        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

        _rg.velocity = transform.rotation * new Vector3(targetVelocity.x, _rg.velocity.y, targetVelocity.y);

        // footstep ayarlari
        float speedRatio = _rg.velocity.magnitude / runSpeed;
        currentFootstepInterval = Mathf.Lerp(baseFootstepInterval, minFootstepInterval, speedRatio);

        if (_rg.velocity.magnitude > 0.1f && groundCheck.isGrounded && !jumpComponent.IsJumping)
        {
            footstepTimer += Time.fixedDeltaTime;
            if (footstepTimer >= currentFootstepInterval)
            {
                AudioManager.Instance.PlayFootstep();
                footstepTimer = 0f;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }
    public void SetPoisonedState(bool poisoned)
    {
        isPoisoned = poisoned;
        if (!poisoned)
        {
            speed = originalWalkSpeed;
            runSpeed = originalRunSpeed;
        }
        else
        {
            speed = poisonedWalkSpeed;
            runSpeed = poisonedRunSpeed;
        }
    }
}
