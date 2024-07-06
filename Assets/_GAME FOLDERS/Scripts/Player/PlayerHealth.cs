using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] private float maxHealth = 100f;
    private float _currentHealth;

    [SerializeField] Image healthBarImage;
    // Prop
    public float MaxHealth => maxHealth;
    public float CurrentHealth => _currentHealth;
    public bool IsDead => _currentHealth <= 0;


    private void Start()
    {
        _currentHealth = maxHealth;
        UpdateHealthBar();
    }
    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        }
        UpdateHealthBar();
        Debug.Log("Player Current Health: " + _currentHealth);
    }
    public void Heal(float amount)
    {
        if (IsDead) return;

        _currentHealth = Mathf.Min(_currentHealth + amount, maxHealth);
        UpdateHealthBar();
        Debug.Log("Player Current Health: " + _currentHealth);
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
        Debug.Log("Player has died!");
        // TODO: Ölüm animasyonu gelecek
    }
}