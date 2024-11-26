using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [SerializeField] int startingHealth = 100;      //This is base Health Points
    [SerializeField] int startingMana = 100;        //This is base mana
    [SerializeField] float timeSinceLastHit = 2f;   //This is base regen timer
    
    private float timer = 0f;       //Timer used to see in-game time
    private PlayerMovement player;  //Player name
    private int currentHealth;      //Grabs Current Health Points
    private int currentMana;        //Grabs Current Mana 
    
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
