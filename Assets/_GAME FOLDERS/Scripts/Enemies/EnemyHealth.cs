using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IHealth
{
    [SerializeField] private float maxHealth = 100f;
    private float _currentHealth;
    private Animator _animator;
    [SerializeField] Image healthBarImage;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => _currentHealth;
    public bool IsDead => _currentHealth <= 0;

    private void Start()
    {
        _currentHealth = maxHealth;
        _animator = GetComponentInChildren<Animator>();
        UpdateHealthBar();
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;

        _currentHealth -= amount;

        if (_currentHealth <= 0)
        {
            Die();
        }
        UpdateHealthBar();
        Debug.Log("Enemy Current Health: " + _currentHealth);
    }
    private void UpdateHealthBar()
    {
        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = _currentHealth / maxHealth;
        }
    }
    public void Die()
    {
        Debug.Log("Enemy has died!");
        _animator.SetTrigger("IsDead");
        Destroy(gameObject, 2.2f);
    }
}
