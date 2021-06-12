
using System.Collections.Generic;
using UnityEngine;

public class DexterMovement : MonoBehaviour
{
    public KeyCode jump = KeyCode.W;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode moveLeft = KeyCode.A;
    public float horizSpeed = 3.0f;
    public float jumpPower;
    public bool ragdolling;
    public bool canGetMagneted;
    float distToWall;
    public LayerMask groundLayer;
    //public float boundY = 2.25f; PUT THIS BACK IN TO LIMIT FROM FALLING OFF-SCREEN

    public GameObject DexterModel;
    public DexterHook Hook;

    private Rigidbody2D rb2d;
    private Collider2D collider;
    private Animator animator;
    // Use this for initialization

    private Dictionary<string, (Quaternion, Vector3)> standingDexter = new Dictionary<string, (Quaternion, Vector3)>();

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponentInChildren<Animator>();
        StopRagdolling();
    }

    void Start()
    {
        ragdolling = false;
        distToWall = collider.bounds.extents.x + 0.1f;
        SaveBoneLocations();
    }

    private void SaveBoneLocations()
    {
        foreach (var bone in DexterModel.GetComponentsInChildren<Transform>())
        {
            if (!standingDexter.ContainsKey(bone.gameObject.name))
            {
                standingDexter.Add(bone.gameObject.name, (bone.localRotation, bone.localPosition));
            }
        }
    }

    private void LoadBoneLocations()
    {
        foreach (var bone in DexterModel.GetComponentsInChildren<Transform>())
        {
            if (standingDexter.ContainsKey(bone.gameObject.name))
            {
                var loc = standingDexter[bone.gameObject.name];
                bone.localRotation = loc.Item1;
                bone.localPosition = loc.Item2;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vel = rb2d.velocity;//velocity is set to current velocity
        bool grounded = GetComponent<CheckGrounded>().grounded;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            canGetMagneted = true;
            animator.SetBool("Mag", true);
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            canGetMagneted = false;
            animator.SetBool("Mag", false);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartRagdolling();
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            StopRagdolling();
        }

        if (!ragdolling)
        {
            
            //if touching ground:
            if (grounded)
            {
                if (Input.GetKey(jump))
                {
                    animator.SetBool("Jumping", true);
                    if (grounded)
                    {
                        vel.y = jumpPower; //placeholder jump height value
                    }

                }
                else
                {
                    animator.SetBool("Falling", false);
                    animator.SetBool("Jumping", false);
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

            if (Input.GetKey(moveRight))
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                RaycastHit2D wallCheck = Physics2D.Raycast(new Vector2(GetComponent<BoxCollider2D>().bounds.center.x, GetComponent<BoxCollider2D>().bounds.center.y - (GetComponent<BoxCollider2D>().bounds.extents.y * 0.8f)), Vector2.right, distToWall + 0.05f, groundLayer);
                if (vel.x <= horizSpeed && wallCheck.collider == null)
                {
                    animator.SetBool("Running", true);
                    vel.x = vel.x + 1;
                }               
            }
            else if (Input.GetKey(moveLeft))
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
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
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }

            if (GetComponent<CheckGrounded>().grounded && vel.y < 0)
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
            //Debug.Log(vel);

        }
        
    }

    void StartRagdolling()
    {
        ragdolling = true;
        animator.SetBool("Ragdoll", true);

        // Disable dexter normal collider
        collider.enabled = false;
        rb2d.isKinematic = true;

        // Enable all of the garbage
        var hingeJoints = DexterModel.GetComponentsInChildren<HingeJoint2D>();
        foreach (var j in hingeJoints) j.enabled = true;

        var limbColliders = DexterModel.GetComponentsInChildren<EdgeCollider2D>();
        foreach (var l in limbColliders) l.isTrigger = false;

        var limbRigidBodies = DexterModel.GetComponentsInChildren<Rigidbody2D>();
        foreach (var r in limbRigidBodies) r.bodyType = RigidbodyType2D.Dynamic;
    }

    void StopRagdolling()
    {
        ragdolling = false;
        animator.SetBool("Ragdoll", false);

        // Enable normal dexter collider
        collider.enabled = true;
        rb2d.isKinematic = false;

        // Disable all of the garbage
        var hingeJoints = DexterModel.GetComponentsInChildren<HingeJoint2D>();
        foreach (var j in hingeJoints) j.enabled = false;

        var limbColliders = DexterModel.GetComponentsInChildren<EdgeCollider2D>();
        foreach (var l in limbColliders) l.isTrigger = true;

        var limbRigidBodies = DexterModel.GetComponentsInChildren<Rigidbody2D>();
        foreach (var r in limbRigidBodies) r.bodyType = RigidbodyType2D.Kinematic;

        LoadBoneLocations();
        canGetMagneted = false; 
    }
}