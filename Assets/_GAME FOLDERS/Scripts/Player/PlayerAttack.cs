using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    private PlayerHealth _playerHealth;
    private Animator _animator;
    private int _currentWeaponIndex = -1;
    private float _nextAttackTime = 0f;
    private float _originalDamage;
    private bool _isPoisoned = false;
    private string _animationName;

    [SerializeField] Camera _cam;
    [SerializeField] Weapon[] weapons;
    [SerializeField] string[] animationsName;
    [SerializeField] Image[] selectedImage;
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] private float poisonedDamage = 5f;
    private void Start()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        _animator = GetComponentInChildren<Animator>();

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(false);
        }

        if (weapons.Length > 0)
        {
            _originalDamage = weapons[0].damage;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.IsGamePaused()) return;

        HandleWeaponSwitching();

        if (Time.time >= _nextAttackTime && Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void HandleWeaponSwitching()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToggleWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ToggleWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ToggleWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && GameManager.Instance.IsItemPurchased("Weapon_4"))
        {
            ToggleWeapon(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && GameManager.Instance.IsItemPurchased("Weapon_5"))
        {
            ToggleWeapon(4);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (_currentWeaponIndex != -1)
                ToggleWeapon(_currentWeaponIndex);
        }
    }

    private void ToggleWeapon(int index)
    {
        if (index == _currentWeaponIndex)
        {
            selectedImage[index].gameObject.SetActive(false);
            weapons[index].gameObject.SetActive(false);
            _currentWeaponIndex = -1;
            selectedImage[index].transform.parent.DOScale(1f, 0.15f);
        }
        else
        {
            if (_currentWeaponIndex != -1)
            {
                weapons[_currentWeaponIndex].gameObject.SetActive(false);
                selectedImage[_currentWeaponIndex].gameObject.SetActive(false);
            }
            for (int i = 0; i < selectedImage.Length; i++)
            {
                selectedImage[i].transform.parent.DOScale(1f, 0.25f);
            }
            _animationName = animationsName[index];
            selectedImage[index].gameObject.SetActive(true);
            selectedImage[index].transform.parent.DOScale(1.2f, 0.25f);
            weapons[index].gameObject.SetActive(true);
            _currentWeaponIndex = index;

        }
    }

    private void Attack()
    {
        if (_playerHealth.IsDead) return;
        if (_currentWeaponIndex == -1) return;

        Weapon currentWeapon = weapons[_currentWeaponIndex];
        _animator.SetTrigger(_animationName);
        _playerHealth.SetAttacking(true);

        float damage = _isPoisoned ? poisonedDamage : currentWeapon.damage;

        Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hitInfo, currentWeapon.attackRange, LayerMask.GetMask("Enemy")))
        {
            EnemyHealth enemyHealth = hitInfo.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }

        _nextAttackTime = Time.time + attackCooldown;

        StartCoroutine(ResetAttackState(0.5f));
    }

    private IEnumerator ResetAttackState(float delay)
    {
        yield return new WaitForSeconds(delay);
        _playerHealth.SetAttacking(false);
    }

    public void SetPoisonedState(bool poisoned)
    {
        _isPoisoned = poisoned;
        if (!poisoned)
        {
            if (weapons.Length > 0)
            {
                weapons[0].damage = _originalDamage;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_cam == null)
        {
            _cam = Camera.main;
            if (_cam == null) return;
        }

        if (_currentWeaponIndex == -1) return;

        Weapon currentWeapon = weapons[_currentWeaponIndex];
        Gizmos.color = Color.red;
        Vector3 rayOrigin = _cam.transform.position;
        Vector3 rayDirection = _cam.transform.forward;
        Gizmos.DrawRay(rayOrigin, rayDirection * currentWeapon.attackRange);
    }
}
