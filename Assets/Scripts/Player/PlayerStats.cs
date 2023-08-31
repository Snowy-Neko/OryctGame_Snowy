using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [SerializeField] int startingHealth = 100;
    [SerializeField] int startingMana = 100;
    [SerializeField] float timeSinceLastHit = 2f;
    
    private float timer = 0f;
    private PlayerMovement player;
    private int currentHealth;
    private int currentMana;
    
    // Start is called before the first frame update
    void Start()
    {
        // anim = GetComponent<Animator>(); use for death animation
        player = GetComponent<PlayerMovement>();
        currentHealth = startingHealth; 
        currentMana = startingMana;

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (timer >= timeSinceLastHit && !GameManager.instance.GameOver)
        {
            if (other.tag == "Weapon")
            {
                takeDamage();
                timer = 0;
            }
        }
    }

    void takeDamage()
    {
        if (currentHealth > 0)
        {
            GameManager.instance.PlayerHit(currentHealth);
            //anim.Play("PlayerDamage");
            currentHealth -= 5;
        }

        if (currentHealth <= 0)
        {
            killPlayer();
        }

        void killPlayer()
        {
            GameManager.instance.PlayerHit(currentHealth);
            player.enabled = false;
        }
    }

}
