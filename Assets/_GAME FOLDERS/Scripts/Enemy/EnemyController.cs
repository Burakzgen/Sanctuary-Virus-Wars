using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] float detectionRadius = 10f;

    [Header("Attack Settings")]
    [SerializeField] float attackRadius = 1.5f;
    [SerializeField] float attackDamage = 20f;
    [SerializeField] float attackCooldown = 2f;

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float runSpeed = 4f;

    [Header("Patrol Settings")]
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] float returnToPatrolDelay = 5f; // Oyuncu kaybolduktan sonra devriyeye dönme gecikmesi
    [SerializeField] float patrolWaitTime = 5f; // Devriye noktalara vardýðýnda bekleme süresi

    private NavMeshAgent _agent;
    private Animator _animator;
    private Transform _player;
    private PlayerHealth _playerHealth;
    private Vector3 _startPosition;
    private bool _isPlayerDetected = false;
    private bool _isAttacking = false;
    private float _lastAttackTime;
    private bool _isPatrolling = false;
    private Coroutine _patrolCoroutine;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerHealth = _player.GetComponent<PlayerHealth>();
        _startPosition = transform.position;
        _agent.speed = moveSpeed;
        _agent.stoppingDistance = attackRadius + 0.4f;

        StartPatrolling();
    }

    private void Update()
    {
        if (_playerHealth.IsDead)
        {
            _isPlayerDetected = false;
            _agent.isStopped = false;
            StartCoroutine(ReturnToPatrolAfterDelay());
            return;
        }

        DetectPlayer();
        AttackPlayer();
        UpdateAnimations();
    }

    private void DetectPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        if (distanceToPlayer <= detectionRadius && !_playerHealth.IsDead)
        {
            if (!_isPlayerDetected)
            {
                _isPlayerDetected = true;
                StopPatrolling();
            }
            _agent.isStopped = false;
            _agent.SetDestination(_player.position);
            _agent.speed = runSpeed;
        }
        else
        {
            if (_isPlayerDetected)
            {
                _isPlayerDetected = false;
                _agent.speed = moveSpeed;
                StartCoroutine(ReturnToPatrolAfterDelay());
            }
        }
    }

    private void AttackPlayer()
    {
        if (!_isPlayerDetected) return;
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        if (distanceToPlayer <= attackRadius)
        {
            _agent.isStopped = true;
            transform.LookAt(_player);

            if (!_isAttacking && Time.time >= _lastAttackTime + attackCooldown)
            {
                StartCoroutine(PerformAttack());
            }
        }
        else
        {
            _agent.isStopped = false;
        }
    }

    private IEnumerator PerformAttack()
    {
        _isAttacking = true;
        _animator.SetTrigger("IsAttack");

        yield return new WaitForSeconds(0.1f);

        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
        if (distanceToPlayer <= attackRadius && !_playerHealth.IsDead)
        {
            IHealth playerHealth = _player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }

        _lastAttackTime = Time.time;
        _isAttacking = false;
    }

    private void StartPatrolling()
    {
        if (_patrolCoroutine != null)
        {
            StopCoroutine(_patrolCoroutine);
        }
        _patrolCoroutine = StartCoroutine(PatrolCoroutine());
    }

    private void StopPatrolling()
    {
        if (_patrolCoroutine != null)
        {
            StopCoroutine(_patrolCoroutine);
        }
        _isPatrolling = false;
    }

    private IEnumerator PatrolCoroutine()
    {
        _isPatrolling = true;
        while (true)
        {
            foreach (Transform point in patrolPoints)
            {
                _agent.isStopped = false;
                _agent.SetDestination(point.position);

                while (_agent.pathPending || _agent.remainingDistance > 0.1f)
                {
                    if (_isPlayerDetected) yield break;
                    yield return null;
                }

                _agent.isStopped = true;
                yield return new WaitForSeconds(patrolWaitTime);
            }
        }
    }

    private IEnumerator ReturnToPatrolAfterDelay()
    {
        yield return new WaitForSeconds(returnToPatrolDelay);
        if (!_isPlayerDetected)
        {
            StartPatrolling();
        }
    }

    private void UpdateAnimations()
    {
        bool isMoving = _agent.velocity.magnitude > 0.1f;
        _animator.SetBool("IsWalking", isMoving && !_isPlayerDetected);
        _animator.SetBool("IsRunning", isMoving && _isPlayerDetected);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
