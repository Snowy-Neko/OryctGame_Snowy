
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //Die animation state
        Debug.Log("Enemy Died!");
        
        //Destroy enemy
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
