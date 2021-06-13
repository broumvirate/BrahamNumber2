using System.Collections;
using UnityEngine;


public class DexterMovement : MonoBehaviour
{
    public KeyCode reach = KeyCode.Tab;
    public KeyCode jump = KeyCode.W;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode moveLeft = KeyCode.A;
    public float horizSpeed = 3.0f;
    public float jumpPower;
    public bool hooked;
    public bool canGetMagneted;

    float distToWall;
    public LayerMask groundLayer;
    //public float boundY = 2.25f; PUT THIS BACK IN TO LIMIT FROM FALLING OFF-SCREEN

    //I put audio here
    public AudioClip dexterJump;
    public AudioSource audio;
    

    public bool reaching;
    private Rigidbody2D rb2d;
    private Collider2D collider;
    private Animator animator;
    private Ragdoll ragdoll;
    public bool recoveryPeriod;


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponentInChildren<Animator>();
        ragdoll = GetComponent<Ragdoll>();
        distToWall = collider.bounds.extents.x + 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vel = rb2d.velocity;//velocity is set to current velocity
        bool grounded = GetComponent<CheckGrounded>().grounded;

        HandleMagnet();
        HandleJump(ref vel);
        HandleMovement(ref vel);

        //this part is shitty, but it prevents fucko mode so don't delete it
        if (grounded && !hooked && recoveryPeriod)
        {
            //Debug.Log("what the fuck");
            recoveryPeriod = false;
            if (Mathf.Abs(transform.rotation.y) > 90)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            else
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        //end fucko mode prevention//

        if (!ragdoll.ragdolling)
        {
            // Gravity and floor
            if (grounded && vel.y < 0)
            {
                vel.y = 0;
                //animator.SetBool("Falling", false);
                rb2d.velocity = vel;
            }
            else
            {
                //animator.SetBool("Falling", true);
                vel.y -= 0.01f;
            }

            rb2d.velocity = vel;
        }
        
    }

    private void HandleMagnet()
    {
        Vector2 vel = rb2d.velocity;//velocity is set to current velocity
        bool grounded = GetComponent<CheckGrounded>().grounded;

        if (Input.GetKey(KeyCode.Tab))
        {
            reaching = true;
            canGetMagneted = true;
            animator.SetBool("Mag", true);
        }
        else
        {
            reaching = false;
            canGetMagneted = false;
            animator.SetBool("Mag", false);
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            canGetMagneted = false;
            animator.SetBool("Mag", false);
        }

    }

    /// <summary>
    /// Adds jump force to the vector given, handles jump and fall animations
    /// </summary>
    /// <param name="vel"></param>
    private void HandleJump(ref Vector2 vel)
    {
        bool grounded = GetComponent<CheckGrounded>().grounded;
        //if touching ground:
        if (!ragdoll.ragdolling)
        {
            if (grounded)
            {
                animator.SetBool("Falling", false);
                if (Input.GetKey(jump) && !reaching)
                {
                    // Add jump force
                    vel.y = jumpPower;
                    animator.SetBool("Jumping", true);
                    PlayJumpNoise();

                }
              
            }
            else
            {
                if (!Input.GetKey(jump))
                {
                    animator.SetBool("Falling", true);
                    animator.SetBool("Jumping", false);
                }
            }
        }
    }

    /// <summary>
    /// Adds any movements to the input given, sets animations
    /// </summary>
    /// <param name="vel"></param>
    private void HandleMovement(ref Vector2 vel)
    {
        if (!ragdoll.ragdolling)
        {
            if (Input.GetKey(moveRight) && !reaching)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                if (!hooked)
                {
                    GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                }
                RaycastHit2D wallCheck = Physics2D.Raycast(new Vector2(GetComponent<BoxCollider2D>().bounds.center.x, GetComponent<BoxCollider2D>().bounds.center.y - (GetComponent<BoxCollider2D>().bounds.extents.y * 0.8f)), Vector2.right, distToWall + 0.05f, groundLayer);
                if (vel.x <= horizSpeed && wallCheck.collider == null)
                {
                    animator.SetBool("Running", true);
                    vel.x = vel.x + 1;
                }               
            }
            else if (Input.GetKey(moveLeft) && !reaching)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                if (!hooked)
                {
                    GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                }
                RaycastHit2D wallCheck = Physics2D.Raycast(new Vector2(GetComponent<BoxCollider2D>().bounds.center.x, GetComponent<BoxCollider2D>().bounds.center.y - (GetComponent<BoxCollider2D>().bounds.extents.y * 0.8f)), Vector2.left, distToWall + 0.05f, groundLayer);
                if (vel.x >= -horizSpeed && wallCheck.collider == null)
                {
                    animator.SetBool("Running", true);
                    vel.x = vel.x - 1;
                }                
            }
            else
            {
                animator.SetBool("Running", false);
                vel.x /= 2;
            }
        }
    }

    private bool playingJump = false;
    private void PlayJumpNoise()
    {
        if (!playingJump)
        {
            audio.PlayOneShot(dexterJump, 0.7f);
            playingJump = true;
            StartCoroutine(FixJump());
        }
    }

    private IEnumerator FixJump()
    {
        yield return new WaitForSeconds(0.1f);
        playingJump = false;
    }
}