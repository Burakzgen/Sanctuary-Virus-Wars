using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float _currentHealth;

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        }

        Debug.Log("Current Health:" + _currentHealth);
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Ölüm iþlemleri burada
    }
}