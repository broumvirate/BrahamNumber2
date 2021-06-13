using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Controller : MonoBehaviour
{
    public float speed = 1f;
    public DexterMovement Dexter;
    public List<MakeLethal> DontMakeLethals = new List<MakeLethal>();


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

    //Audio
    public AudioClip skeleTack;
    public AudioClip skeleDeath;
    public AudioSource audio;


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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var leth = collider.gameObject.GetComponent<MakeLethal>();
        if (leth != null && !DontMakeLethals.Contains(leth))
        {
            StartCoroutine(Kill());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var leth = collision.gameObject.GetComponent<MakeLethal>();
        if (leth != null && !DontMakeLethals.Contains(leth))
        {
            StartCoroutine(Kill());
        }
    }

    public IEnumerator Kill()
    {
        animator.SetBool("Dying", true);
        audio.PlayOneShot(skeleDeath, 0.8f);
        yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    }

    #endregion

    public void playAttack()
    {
        Debug.Log(skeleTack.name);
        audio.PlayOneShot(skeleTack, 0.5f);
    }
}
