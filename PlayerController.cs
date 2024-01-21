using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;
    public float collisionOffset = 0.02f;
    Vector2 movementInput;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    Vector2 targetPosition;
    bool canMove = true;
    Vector2 velocity;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (canMove){
            Vector2 movement = movementInput.magnitude > 0.1f ? movementInput.normalized : Vector2.zero;
            if (movementInput != Vector2.zero){
                bool success = TryMove(movement);

                if (!success)
                {
                    success = TryMove(new Vector2(movement.x, 0));

                    if (!success)
                    {
                        success = TryMove(new Vector2(0, movement.y));
                    }
                }
                animator.SetBool("IsMoving", success);
            } else {
                animator.SetBool("IsMoving", false);
            }

            if(movement.x > 0){
                spriteRenderer.flipX = false;
                swordAttack.attackDirection = SwordAttack.AttackDirection.right;
            } else if (movement.x < 0){
                spriteRenderer.flipX = true;
                swordAttack.attackDirection = SwordAttack.AttackDirection.left;
            }
            if(movement.y > 0){
                if(movement.x == 0){
                    animator.SetBool("MovingDown", false);
                    animator.SetBool("MovingRight", false);
                    animator.SetBool("MovingUp", true);
                    swordAttack.attackDirection = SwordAttack.AttackDirection.up;
                } else {
                    animator.SetBool("MovingDown", false);
                    animator.SetBool("MovingUp", false);
                    animator.SetBool("MovingRight", true);
                }
            } else if (movement.y < 0){
                if(movement.x == 0){
                    animator.SetBool("MovingUp", false);
                    animator.SetBool("MovingRight", false);
                    animator.SetBool("MovingDown", true);
                    swordAttack.attackDirection = SwordAttack.AttackDirection.down;
                } else {
                    animator.SetBool("MovingDown", false);
                    animator.SetBool("MovingUp", false);
                    animator.SetBool("MovingRight", true);
                } 
            } else if (movement.x == 0 && movement.y == 0){
                animator.SetBool("MovingDown", false);
                animator.SetBool("MovingUp", false);
                animator.SetBool("MovingRight", false);
            }
        }
    }

    private bool TryMove(Vector2 direction)
    {
        int count = rb.Cast(direction,
        movementFilter,castCollisions, 
        moveSpeed * Time.fixedDeltaTime + collisionOffset);
        if (count == 0){
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        } else {
            return false;
        }
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire()
    {
        animator.SetTrigger("SwordAttack");
    }

    public void PerformSwordAttack ()
    {
        Vector2 movement = movementInput.magnitude > 0.1f ? movementInput.normalized : Vector2.zero;
        if(movement.x > 0){
            swordAttack.AttackRight();
        } else if (movement.x < 0){
            swordAttack.AttackLeft();
        }
        if(movement.y > 0){
            if(movement.x == 0){
                swordAttack.AttackUp();
            }
        } else if (movement.y < 0){
            if(movement.x == 0){
                swordAttack.AttackDown();
            }
        } else if (movement.x == 0 && movement.y == 0){
            swordAttack.AttackDown();
        }
    }
    public void StopSwordAttack()
    {
        Debug.Log("func");
        swordAttack.StopAttack();
    }
}
