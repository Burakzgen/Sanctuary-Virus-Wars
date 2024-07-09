using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IHealth
{
    [SerializeField] private float maxHealth = 100f;
    private float _currentHealth;
    private Animator _animator;
    private bool _isDead = false;
    [SerializeField] Image healthBarImage;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => _currentHealth;
    public bool IsDead => _isDead;

    private void Start()
    {
        _currentHealth = maxHealth;
        _animator = GetComponentInChildren<Animator>();
        UpdateHealthBar();
    }

    public void TakeDamage(float amount)
    {
        if (_isDead) return;

        _currentHealth -= amount;

        UpdateHealthBar();
        if (_currentHealth <= 0)
        {
            Die();
        }
        Debug.Log("Enemy Current Health: " + _currentHealth);
    }
    private void UpdateHealthBar()
    {
        if (_isDead) return;

        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = _currentHealth / maxHealth;
        }
    }
    public void Die()
    {
        Debug.Log("Enemy has died!");
        _isDead = true;
        _animator.SetTrigger("IsDead");
        Destroy(gameObject, 2.2f);
    }
}
