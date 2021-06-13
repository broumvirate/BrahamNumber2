using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Controller : MonoBehaviour
{
    public float speed = 1f;
    public float playerDetectionRange = 20f;
    public float playerAttackRange = 0f;

    public DexterMovement Dexter;


    // Behavior - Walking
    public bool startWalking = false;
    private bool isWalking = true;
    private bool facingLeft = false;
    public bool walkTowardsPlayer = false;

    // Behavior - attack
    public bool attackPlayer = false;
    public float attackCooldown = 1f;
    public bool canAttack = true;

    private Animator animator;
    private Rigidbody2D rb2d;
    private BoxCollider2D collider;

    public LayerMask groundLayer;
    private float distToWall;


    // Start is called before the first frame update
    void Start()
    {
        isWalking = startWalking;
        rb2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponentInChildren<Animator>();
        distToWall = collider.bounds.extents.x + 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(walkTowardsPlayer) StrollsAtYou();
        HandleMovement();
    }

    #region SnootsAtYou

    void StrollsAtYou()
    {
        var dexterLoc = Dexter.GetComponent<Transform>();
        var distance = Vector3.Distance(dexterLoc.position, rb2d.position);
        if (distance < playerDetectionRange || (attackPlayer && distance > playerAttackRange && distance < playerDetectionRange))
        {
            var dexterIsLeft = rb2d.position.x - dexterLoc.position.x > 0;
            isWalking = true;
            facingLeft = dexterIsLeft;
        }
        else
        {
            isWalking = startWalking;

            if (attackPlayer && distance < playerDetectionRange)
            {
                Snoot();
            }
        }
    }

    void Snoot()
    {
        if (canAttack)
        {
            Debug.Log("Attacked");
            animator.SetTrigger("Attack");
            StartCoroutine(SnootCooldown());
        }
    }

    public IEnumerator SnootCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    #endregion

    #region Walking
    private void HandleMovement()
    {
        var vel = rb2d.velocity;
        if (isWalking)
        {
            if (facingLeft) WalkLeft(ref vel);
            else WalkRight(ref vel);
        }
        else
        {
            animator.SetBool("Running", false);
        }

        rb2d.velocity = vel;
    }

    private void WalkRight(ref Vector2 vel)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        RaycastHit2D wallCheck = Physics2D.Raycast(new Vector2(GetComponent<BoxCollider2D>().bounds.center.x, GetComponent<BoxCollider2D>().bounds.center.y - (GetComponent<BoxCollider2D>().bounds.extents.y * 0.8f)), Vector2.left, distToWall + 0.05f, groundLayer);
        if (vel.x <= speed && wallCheck.collider == null)
        {
            animator.SetBool("Running", true);
            vel.x = vel.x + 1;
        } 
    }

    private void WalkLeft(ref Vector2 vel)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        RaycastHit2D wallCheck = Physics2D.Raycast(new Vector2(GetComponent<BoxCollider2D>().bounds.center.x, GetComponent<BoxCollider2D>().bounds.center.y - (GetComponent<BoxCollider2D>().bounds.extents.y * 0.8f)), Vector2.right, distToWall + 0.05f, groundLayer);
        if (vel.x >= -speed && wallCheck.collider == null)
        {
            animator.SetBool("Running", true);
            vel.x = vel.x - 1;
        } 
    }
    
    #endregion
}
