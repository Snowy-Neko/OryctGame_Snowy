using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    public int attackDamage = 40;
    public float attackSpeed = 2f;
    float nextAttackTime = 0f;
    public float attackRange = 3.5f;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Melee();
                nextAttackTime = Time.time + 1f / attackSpeed;
                Debug.Log("Swing");
            }
        }
    }

    void Melee()
    {
        //Need to pass bool to read for animations
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers); //Detecting all enemies at attack point to damage

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            Debug.Log("got they ass lmao"); //You can add a "+ enemy.name" to see all enemies hit 
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
