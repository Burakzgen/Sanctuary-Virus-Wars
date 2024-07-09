using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] private float maxHealth = 100f;
    private float _currentHealth;
    private bool _isAttacking = false;

    [SerializeField] Image healthBarImage;
    [SerializeField] Image poisonBarImage;

    [SerializeField] private float maxPoisonLevel = 100f;
    private float _currentPoisonLevel;

    // Poison effects
    private float poisonEffectCooldown = 5f;
    private float lastPoisonEffectTime;
    private bool isPoisoned = false;

    // Properties
    public float MaxHealth => maxHealth;
    public float CurrentHealth => _currentHealth;
    public bool IsDead => _currentHealth <= 0;
    public bool IsAttacking => _isAttacking;
    public float MaxPoisonLevel => maxPoisonLevel;
    public float CurrentPoisonLevel => _currentPoisonLevel;

    private void Start()
    {
        _currentHealth = maxHealth;
        _currentPoisonLevel = maxPoisonLevel;
        UpdateHealthBar();
        UpdatePoisonBar();
    }

    private void Update()
    {
        if (isPoisoned && Time.time >= lastPoisonEffectTime + poisonEffectCooldown)
        {
            ApplyPoisonEffect();
            lastPoisonEffectTime = Time.time;
        }
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

    public void TakePoisonDamage(float amount)
    {
        _currentPoisonLevel -= amount;
        if (_currentPoisonLevel <= 0)
        {
            _currentPoisonLevel = 0;
            isPoisoned = true;
        }
        UpdatePoisonBar();
        Debug.Log("Player Current Poison Level: " + _currentPoisonLevel);
    }

    public void CurePoison(float amount)
    {
        _currentPoisonLevel += amount;
        if (_currentPoisonLevel > maxPoisonLevel)
        {
            _currentPoisonLevel = maxPoisonLevel;
        }
        if (_currentPoisonLevel > 0)
        {
            isPoisoned = false;
        }
        UpdatePoisonBar();
        Debug.Log("Player Current Poison Level: " + _currentPoisonLevel);
    }

    private void ApplyPoisonEffect()
    {
        // Zehirlenme efektleri burada uygulanacak
        _currentHealth -= 5f; // Örnek olarak her 5 saniyede bir 5 saðlýk azaltma
        UpdateHealthBar();
        if (_currentHealth <= 0)
        {
            Die();
        }
        Debug.Log("Poison effect applied. Player Current Health: " + _currentHealth);
    }

    private void UpdateHealthBar()
    {
        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = _currentHealth / maxHealth;
        }
    }

    private void UpdatePoisonBar()
    {
        if (poisonBarImage != null)
        {
            poisonBarImage.fillAmount = _currentPoisonLevel / maxPoisonLevel;
        }
    }

    public void Die()
    {
        Debug.Log("Player has died!");
        // TODO: Ölüm animasyonu gelecek
    }

    public void SetAttacking(bool isAttacking)
    {
        _isAttacking = isAttacking;
    }
}
