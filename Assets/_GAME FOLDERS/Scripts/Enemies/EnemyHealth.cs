using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IHealth
{
    [SerializeField] private float maxHealth = 100f;
    private float _currentHealth;
    private Animator _animator;
    private bool _isDead = false;
    [SerializeField] Image healthBarImage;
    private EnemyType enemyType;

    // Interact buff
    [SerializeField] bool dropBuff = false;
    [SerializeField] GameObject buffPrefab;
    [SerializeField] float buffDropChance = 0.5f;

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
        if (_isDead) return;

        Debug.Log("Enemy has died!");
        _isDead = true;
        _animator.SetTrigger("IsDead");
        if (dropBuff)
            DropBuff();


        PlayerPrefsManager.IncrementZombieKillCount();
        GameManager.Instance.UpdateZombieCountUI();
        EnemySpawnManager.Instance.RespawnEnemy(enemyType);
        // VFX efekt gelebilir.
        Destroy(gameObject, 2.8f);
    }

    private void DropBuff()
    {
        if (buffPrefab != null && Random.value <= buffDropChance)
        {
            Instantiate(buffPrefab, transform.position, Quaternion.identity);
        }
    }
}
