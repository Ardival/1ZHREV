using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public Collider2D swordCollider;
    public float damage = 2;
    public enum AttackDirection {
        left, right, up, down
    }

    public AttackDirection attackDirection;
    Vector2 attackOffset;

    private void Start()
    {
        attackOffset = transform.position;
    }

   public void AttackRight()
   {
    swordCollider.enabled = true;
    transform.localPosition = attackOffset;
    Invoke("StopAttack", 0.2f);
   }
   public void AttackLeft()
   {
    swordCollider.enabled = true;
    transform.localPosition = new Vector3(attackOffset.x * -1, attackOffset.y);
    Invoke("StopAttack", 0.2f);
   }
   public void AttackUp()
   {
    swordCollider.enabled = true;
    Invoke("StopAttack", 0.2f);
   }
   public void AttackDown()
   {
    swordCollider.enabled = true;
    Invoke("StopAttack", 0.2f);
   }
   public void StopAttack()
   {
    Debug.Log("STOOOOOOP");
    swordCollider.enabled = false;
   }
   private void OnTriggerEnter2D(Collider2D other)
   {
    Debug.Log("Trigger entered");
    if(other.tag == "Enemy"){
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null){
            Debug.Log("Enemy hit!");
            enemy.Health -=damage;
        }
    }
   }
}
