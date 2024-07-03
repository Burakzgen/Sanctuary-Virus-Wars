using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float detectionRadius = 10f;
    [SerializeField] float attackRadius = 2f;
    [SerializeField] float damage = 10f;

    private Transform _player;
    private bool _isPlayerDetected = false;
    private bool _isAttacking = false;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _animator = GetComponentInChildren<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        SphereCollider detectionCollider = gameObject.AddComponent<SphereCollider>();
        detectionCollider.radius = detectionRadius;
        detectionCollider.isTrigger = true;
    }

    private void Update()
    {
        if (_isPlayerDetected && !_isAttacking)
        {
            _navMeshAgent.SetDestination(_player.position);

            if (Vector3.Distance(transform.position, _player.position) <= attackRadius)
            {
                StartCoroutine(AttackPlayer());
            }
        }
        else
        {
            _navMeshAgent.SetDestination(transform.position);
        }
    }

    private IEnumerator AttackPlayer()
    {
        _isAttacking = true;
        // Saldýrý animasyonu gelecek
        _animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
        if (Vector3.Distance(transform.position, _player.position) <= attackRadius)
        {
            // Oyuncuya hasar verilecek
        }
        _isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerDetected = true;
            _animator.SetBool("Move", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerDetected = false;
            _animator.SetBool("Move", false);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
