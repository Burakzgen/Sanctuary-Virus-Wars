using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackDamage = 20f;
    public float attackRange = 2f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        //Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, LayerMask.GetMask("Enemy"));

        //foreach (Collider enemy in hitEnemies)
        //{
        //    EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        //    if (enemyHealth != null)
        //    {
        //        enemyHealth.TakeDamage(attackDamage);
        //    }
        //}
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}