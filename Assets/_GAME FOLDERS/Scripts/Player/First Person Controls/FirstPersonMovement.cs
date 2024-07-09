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
        IsRunning = canRun && Input.GetKey(runningKey);

        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

        _rg.velocity = transform.rotation * new Vector3(targetVelocity.x, _rg.velocity.y, targetVelocity.y);
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
