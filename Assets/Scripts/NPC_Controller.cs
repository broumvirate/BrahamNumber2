using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Controller : MonoBehaviour
{
    public float speed = 1f;

    public DexterMovement Dexter;


    // Behavior - Walking
    private bool isWalking = false;
    private bool facingLeft = false;
    public bool startWalking = false;
    public bool walkTowardsPlayer = false;
    public float playerDetectionRange = 20f;

    // Behavior - Attack
    public bool attackPlayer = false;
    public float attackCooldown = 1f;
    public float playerAttackRange = 4f;
    private bool canAttack = true;

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
        if (distance <= playerDetectionRange && distance > (attackPlayer ? playerAttackRange : 0f))
        {
            var dexterIsLeft = rb2d.position.x - dexterLoc.position.x < 0;
            isWalking = true;
            facingLeft = dexterIsLeft;
        }
        else if (attackPlayer && distance < playerAttackRange)
        {
            Snoot();
            isWalking = false;
        }
        else
        {
            isWalking = startWalking;
        }
    }

    // TODO: Make snoot fucking work better
    void Snoot()
    {
        if (canAttack)
        {
            animator.SetBool("Attack", true);
            StartCoroutine(SnootCooldown());
        }
    }

    public IEnumerator SnootCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        animator.SetBool("Attack", false);
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

    private void WalkLeft(ref Vector2 vel)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        RaycastHit2D wallCheck = Physics2D.Raycast(new Vector2(GetComponent<BoxCollider2D>().bounds.center.x, GetComponent<BoxCollider2D>().bounds.center.y - (GetComponent<BoxCollider2D>().bounds.extents.y * 0.8f)), Vector2.left, distToWall + 0.05f, groundLayer);
        if (vel.x <= speed && wallCheck.collider == null)
        {
            animator.SetBool("Running", true);
            vel.x = vel.x + 1;
        } 
    }

    private void WalkRight(ref Vector2 vel)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        RaycastHit2D wallCheck = Physics2D.Raycast(new Vector2(GetComponent<BoxCollider2D>().bounds.center.x, GetComponent<BoxCollider2D>().bounds.center.y - (GetComponent<BoxCollider2D>().bounds.extents.y * 0.8f)), Vector2.right, distToWall + 0.05f, groundLayer);
        if (vel.x >= -speed && wallCheck.collider == null)
        {
            animator.SetBool("Running", true);
            vel.x = vel.x - 1;
        } 
    }
    
    #endregion

    #region Killing them

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<MakeLethal>() != null)
        {
            StartCoroutine(Kill());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<MakeLethal>() != null)
        {
            StartCoroutine(Kill());
        }
    }

    public IEnumerator Kill()
    {
        animator.SetBool("Dying", true);
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
    }

    #endregion
}
