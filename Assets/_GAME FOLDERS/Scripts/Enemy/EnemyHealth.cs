using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth
{
    [SerializeField] float maxHealth = 100f;
    float _currentHealth;

    // Prop
    public float CurrentHealth => _currentHealth;
    public float MaxHealth => maxHealth;
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
        Debug.Log("Enemy Current Health:" + _currentHealth);
    }
    public void Die()
    {
        Debug.Log("Enemy has died!");
        // TODO: Ölüm animasyonu gelecek

        Destroy(gameObject);
    }
}
