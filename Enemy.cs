using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 0.2f;
    public float health = 4;
    public int damage = 2;
    private float distance;
    public GameObject player;

    void Update()
    {
       distance = Vector2.Distance(transform.position, player.transform.position);
       Vector2 direction = player.transform.position - transform.position;
       transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("he hit me");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("he hit me");
    
            // PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            // if (playerHealth != null)
            // {
            //     playerHealth.TakeDamage(damageAmount);
            // }
        }
    }
   public float Health {
    set{
        health = value;
        if(health <= 0){
            Defeated();
        }
    }
    get {
        return health;
    }
   }

   public void Defeated(){
    Debug.Log("Enemy defeated");
    Destroy(gameObject);
   }
}
