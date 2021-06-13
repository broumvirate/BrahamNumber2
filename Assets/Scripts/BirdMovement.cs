using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    #region Variables

    public float horizSpeed;
    public float jumpVelocity;
    public LayerMask groundLayer;

    private float distToWall;
    
    private Rigidbody2D rb;
    private Collider2D collider;
    private Animator animator;

    public bool canMove = true;

    private KeyCode jump = KeyCode.UpArrow;
    private KeyCode moveLeft = KeyCode.LeftArrow;
    private KeyCode moveRight = KeyCode.RightArrow;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        distToWall = collider.bounds.extents.x + 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(jump)) Jump();

        Move();

    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
        animator.SetTrigger("Jump");
    }

    void Move()
    {
        if (canMove)
        {
            Vector2 vel = rb.velocity;//velocity is set to current velocity
            bool isGrounded = GetComponent<CheckGrounded>().grounded;

            if (Input.GetKey(moveRight) && !isGrounded)
            {
                RaycastHit2D wallCheck = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - (GetComponent<BoxCollider2D>().bounds.extents.y * 0.8f)), Vector2.right, distToWall + 0.05f, groundLayer);
                if (vel.x <= horizSpeed && wallCheck.collider == null)
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
                    vel.x = vel.x + 1;
                }
                    
            }
            else if (Input.GetKey(moveLeft) && !isGrounded)
            {
                RaycastHit2D wallCheck = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - (GetComponent<BoxCollider2D>().bounds.extents.y * 0.8f)), Vector2.left, distToWall + 0.05f, groundLayer);
                if (vel.x >= -horizSpeed && wallCheck.collider == null)
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0,180,0));
                    vel.x = vel.x - 1;
                }
                    
            }
            else
            {
                vel.x /= 1.01f;
            }

            rb.velocity = vel;
        }
    }
}
