using UnityEngine;

public class FirstPersonJump : MonoBehaviour
{
    // Private
    Rigidbody _rb;
    [SerializeField] FirstPersonGroundCheck _groundCheck;

    [SerializeField] float jumpStrength = 2;
    [SerializeField] event System.Action Jumped;
    public bool IsJumping { get; private set; }
    void Reset()
    {
        _groundCheck = GetComponentInChildren<FirstPersonGroundCheck>();
    }
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    void LateUpdate()
    {
        if (GameManager.Instance.IsGamePaused()) return;

        if (Input.GetButtonDown("Jump") && (!_groundCheck || _groundCheck.isGrounded))
        {
            _rb.AddForce(Vector3.up * 100 * jumpStrength);
            Jumped?.Invoke();
            AudioManager.Instance.PlaySFX(AudioManager.Instance.jumpSound); IsJumping = true;
        }
        if (IsJumping && _groundCheck.isGrounded)
        {
            IsJumping = false;
        }
    }
}
