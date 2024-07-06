using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Weapon[] weapons; // Karakterin sahip olduðu silahlar
    [SerializeField] float attackCooldown = 1f; // Saldýrý gecikme süresi
    private int _currentWeaponIndex = -1;

    private PlayerHealth _playerHealth;
    private Camera _cam;
    private float _nextAttackTime = 0f;
    private Animator _animator;

    private void Start()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        _cam = Camera.main;
        _animator = GetComponentInChildren<Animator>();

        // Tüm silahlarý baþta kapat
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
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
    }

    private void ToggleWeapon(int index)
    {
        if (index == _currentWeaponIndex)
        {
            weapons[index].gameObject.SetActive(false);
            _currentWeaponIndex = -1;
        }
        else
        {
            if (_currentWeaponIndex != -1)
            {
                weapons[_currentWeaponIndex].gameObject.SetActive(false);
            }
            weapons[index].gameObject.SetActive(true);
            _currentWeaponIndex = index;
        }
    }

    private void Attack()
    {
        if (_playerHealth.IsDead) return;
        if (_currentWeaponIndex == -1) return;

        Weapon currentWeapon = weapons[_currentWeaponIndex];
        _animator.SetTrigger("Attack");
        _playerHealth.SetAttacking(true); // Saldýrý baþladýðýnda

        Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hitInfo, currentWeapon.attackRange, LayerMask.GetMask("Enemy")))
        {
            EnemyHealth enemyHealth = hitInfo.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(currentWeapon.damage);
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
