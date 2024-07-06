using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] private float maxHealth = 100f;
    private float _currentHealth;

    // Prop
    public float MaxHealth => maxHealth;
    public float CurrentHealth => _currentHealth;
    public bool IsDead => _currentHealth <= 0;


    private void Start()
    {
        _currentHealth = maxHealth;
    }
    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        }

        Debug.Log("Player Current Health: " + _currentHealth);
    }
    public void Heal(float amount)
    {
        if (IsDead) return;

        _currentHealth = Mathf.Min(_currentHealth + amount, maxHealth);
        Debug.Log("Player Current Health: " + _currentHealth);
    }
    public void Die()
    {
        Debug.Log("Player has died!");
        // TODO: Ölüm animasyonu gelecek
    }
}