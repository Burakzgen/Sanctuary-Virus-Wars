using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public EnemyType enemyType;

    [Header("General Settings")]
    [SerializeField] float detectionRadius = 10f;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float runSpeed = 4f;
    [SerializeField] float stoppingDistance = 2f;
    [SerializeField] CanvasGroup healthCanvasGroup;
    [Header("Attack Settings")]
    [SerializeField] float attackRadius = 1.5f;
    [SerializeField] float attackDamage = 20f;
    [SerializeField] float attackCooldown = 2f;

    [Header("Patrol Settings")]
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] float patrolWaitTime = 5f;

    [Header("Poison Settings")]
    [SerializeField] float poisonRadius = 3f;
    [SerializeField] float poisonDamage = 5f;
    [SerializeField] float poisonDuration = 5f;
    [SerializeField] float poisonCooldown = 10f;
    [SerializeField] GameObject particalEffect;

    private NavMeshAgent _agent;
    private Animator _animator;
    private Transform _player;
    private Collider _collider;
    private EnemyHealth _myHealth;
    private PlayerHealth _playerHealth;
    private Vector3 _startPosition;
    private bool _isPlayerDetected = false;
    private bool _isAttacking = false;
    private float _lastActionTime;
    private int _currentPatrolIndex = 0;
    private Coroutine _poisonCoroutine;
    private EnemyAudio _enemyAudio;

    private void Start()
    {
        InitializeEnemy();
    }

    private void Update()
    {
        if (_myHealth.IsDead)
        {
            HandleDeath();
            return;
        }

        if (_playerHealth.IsDead)
        {
            _isPlayerDetected = false;
            _agent.stoppingDistance = 0f;
            _agent.SetDestination(_startPosition);
        }
        else
        {
            DetectPlayer();
        }

        PerformAction();
        UpdateAnimations();
    }
    public void Init(EnemyType type)
    {
        enemyType = type;
    }
    private void InitializeEnemy()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _collider = GetComponent<Collider>();
        _playerHealth = _player.GetComponent<PlayerHealth>();
        _myHealth = GetComponent<EnemyHealth>();
        _startPosition = transform.position;
        _agent.speed = moveSpeed;
        _enemyAudio = GetComponent<EnemyAudio>();

        if (enemyType == EnemyType.Patroller)
        {
            StartCoroutine(PatrolCoroutine());
        }
        else if (enemyType == EnemyType.Stable || enemyType == EnemyType.Poisoner)
        {
            _agent.isStopped = true;
        }
        _enemyAudio.PlayIdleSound();
    }
    private void DetectPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        if (distanceToPlayer <= detectionRadius && !_playerHealth.IsDead)
        {
            healthCanvasGroup.DOFade(1, 0.25f);
            _isPlayerDetected = true;
            _agent.stoppingDistance = stoppingDistance;
            if (enemyType != EnemyType.Poisoner)
            {
                _agent.isStopped = false;
                _agent.SetDestination(_player.position);
                _agent.speed = runSpeed;
            }
        }
        else
        {
            healthCanvasGroup.DOFade(0, 0.25f);
            _isPlayerDetected = false;
            _agent.stoppingDistance = 0f;
            _agent.speed = moveSpeed;
            if (enemyType == EnemyType.Patroller)
            {
                if (!_agent.pathPending && _agent.remainingDistance < 0.1f)
                {
                    MoveToNextPatrolPoint();
                }
            }
            else if (enemyType != EnemyType.Stable && enemyType != EnemyType.Poisoner)
            {
                _agent.SetDestination(_startPosition);
            }

        }
    }

    private void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
            return;

        _agent.SetDestination(patrolPoints[_currentPatrolIndex].position);
        _currentPatrolIndex = (_currentPatrolIndex + 1) % patrolPoints.Length;
    }

    private void PerformAction()
    {
        if (_playerHealth.IsDead)
        {
            if (enemyType == EnemyType.Patroller)
            {
                if (!_agent.pathPending && _agent.remainingDistance < 0.1f)
                {
                    MoveToNextPatrolPoint();
                }
            }
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        switch (enemyType)
        {
            case EnemyType.Attacker:
            case EnemyType.Stable:
                if (_isPlayerDetected && !_isAttacking)
                {
                    if (distanceToPlayer <= attackRadius)
                    {
                        _collider.isTrigger = true;
                        _agent.isStopped = true;
                        transform.LookAt(_player);
                        PerformAttack();
                    }
                    else
                    {
                        _collider.isTrigger = false;
                        _agent.isStopped = false;
                    }
                }
                break;
            case EnemyType.Patroller:
                if (_isPlayerDetected && !_isAttacking)
                {
                    if (distanceToPlayer <= attackRadius)
                    {
                        _collider.isTrigger = true;
                        _agent.isStopped = true;
                        transform.LookAt(_player);
                        PerformAttack();
                    }
                    else
                    {
                        _collider.isTrigger = false;
                        _agent.isStopped = false;
                    }
                }
                else
                {
                    if (!_agent.pathPending && _agent.remainingDistance < 0.1f)
                    {
                        MoveToNextPatrolPoint();
                    }
                }
                break;
            case EnemyType.Poisoner:
                if (_isPlayerDetected && distanceToPlayer <= poisonRadius)
                {
                    transform.LookAt(_player);
                    _playerHealth.EnterPoisonZone();
                    if (_poisonCoroutine == null)
                    {
                        _poisonCoroutine = StartCoroutine(ApplyPoisonOverTime());
                    }
                }
                else if (!_isPlayerDetected || distanceToPlayer > poisonRadius)
                {
                    _playerHealth.ExitPoisonZone();
                    if (_poisonCoroutine != null)
                    {
                        StopCoroutine(_poisonCoroutine);
                        StartCoroutine(PoissonEffectDelay());
                        _poisonCoroutine = null;
                    }
                }
                break;
        }
    }

    private void PerformAttack()
    {
        if (Time.time >= _lastActionTime + attackCooldown)
        {
            StartCoroutine(AttackCoroutine());
        }
    }
    private IEnumerator PoissonEffectDelay()
    {
        yield return new WaitForSeconds(2.8f);
        if (particalEffect != null)
            particalEffect.SetActive(false);
    }
    private IEnumerator AttackCoroutine()
    {
        _isAttacking = true;
        _animator.SetTrigger("IsAttack");
        _enemyAudio.PlayAttackSound();
        yield return new WaitForSeconds(0.25f);

        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
        if (distanceToPlayer <= attackRadius && !_playerHealth.IsDead)
        {
            float actualDamage = attackDamage;
            _playerHealth.TakeDamage(actualDamage);
            Debug.Log($"Enemy dealt {actualDamage} damage to player");
        }

        _lastActionTime = Time.time;
        _isAttacking = false;

        yield return new WaitForSeconds(attackCooldown - 0.5f);
    }

    private IEnumerator PatrolCoroutine()
    {
        while (true)
        {
            if (!_isPlayerDetected)
            {
                _agent.SetDestination(patrolPoints[_currentPatrolIndex].position);

                while (_agent.pathPending || _agent.remainingDistance > 0.1f)
                {
                    if (_isPlayerDetected) break;
                    yield return null;
                }

                if (!_isPlayerDetected)
                {
                    yield return new WaitForSeconds(patrolWaitTime);
                    _currentPatrolIndex = (_currentPatrolIndex + 1) % patrolPoints.Length;
                }
            }
            yield return null;
        }
    }

    private IEnumerator ApplyPoisonOverTime()
    {
        if (particalEffect != null)
            particalEffect.SetActive(true);
        while (Vector3.Distance(transform.position, _player.position) <= poisonRadius)
        {
            _animator.SetTrigger("IsPoisoning");
            // TODO Ses gelebilir
            _playerHealth.TakePoisonDamage(poisonDamage);
            yield return new WaitForSeconds(1f);
        }
    }

    private void HandleDeath()
    {
        if (_poisonCoroutine != null)
        {
            StopCoroutine(_poisonCoroutine);

            if (particalEffect != null)
                particalEffect.SetActive(false);
            _poisonCoroutine = null;
        }
        _playerHealth.ExitPoisonZone();
        _agent.isStopped = true;
        _collider.enabled = false;

        // TODO: Düþmanýn diðer iþlemlerini burada durdurulacak
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

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, poisonRadius);
    }
}
