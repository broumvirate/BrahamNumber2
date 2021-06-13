
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Experimental.U2D.IK;


public class DexterMovement : MonoBehaviour
{
    public KeyCode reach = KeyCode.Tab;
    public KeyCode jump = KeyCode.W;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode moveLeft = KeyCode.A;
    public float horizSpeed = 3.0f;
    public float yoinkLength = 0.4f;
    public float jumpPower;
    public bool ragdolling;
    public bool canGetMagneted;
    float distToWall;
    public LayerMask groundLayer;
    //public float boundY = 2.25f; PUT THIS BACK IN TO LIMIT FROM FALLING OFF-SCREEN

    public GameObject DexterModel;
    public DexterHook Hook;

    public bool reaching;
    private Rigidbody2D rb2d;
    private Collider2D collider;
    private Animator animator;
    // Use this for initialization

    private Dictionary<string, (Vector3, Quaternion)> standingDexter = new Dictionary<string, (Vector3, Quaternion)>();


    private enum YoinkMode
    {
        Off,
        Teleport,
        Lerp
    }

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponentInChildren<Animator>();
        ragdolling = false;
        StopRagdolling(YoinkMode.Off);
    }

    void Start()
    {
        distToWall = collider.bounds.extents.x + 0.1f;
        SaveBoneLocations();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vel = rb2d.velocity;//velocity is set to current velocity
        bool grounded = GetComponent<CheckGrounded>().grounded;

        HandleMagnet();
        HandleJump(ref vel);
        HandleMovement(ref vel);

        if (!ragdolling)
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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartRagdolling();
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            StopRagdolling(YoinkMode.Lerp);
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
        if (!ragdolling)
        {
            if (grounded)
            {
                animator.SetBool("Falling", false);
                if (Input.GetKey(jump) && !reaching)
                {
                    // Add jump force
                    vel.y = jumpPower;
                    animator.SetBool("Jumping", true);
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
        if (!ragdolling)
        {
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
            else if (Input.GetKey(moveLeft) && !reaching)
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
            }
        }
    }

    #region Ragdoll

    /// <summary>
    /// Sets all of the current bone locations to the StandingDexter dictionary
    /// </summary>
    private void SaveBoneLocations()
    {
        foreach (var bone in DexterModel.GetComponentsInChildren<Transform>())
        {
            if (!standingDexter.ContainsKey(bone.gameObject.name))
            {
                standingDexter.Add(bone.gameObject.name, (bone.localPosition, bone.localRotation));
            }
        }
    }

    /// <summary>
    /// Dexter go wheee
    /// </summary>
    void StartRagdolling()
    {
        ragdolling = true;
        rb2d.velocity = Vector2.zero;
        animator.SetBool("Ragdoll", true);

        // Disable dexter normal collider
        collider.isTrigger = true;
        rb2d.isKinematic = true;

        // Enable all of the garbage
        var hingeJoints = DexterModel.GetComponentsInChildren<HingeJoint2D>();
        foreach (var j in hingeJoints) j.enabled = true;

        var limbColliders = DexterModel.GetComponentsInChildren<EdgeCollider2D>();
        foreach (var l in limbColliders) l.isTrigger = false;

        var limbRigidBodies = DexterModel.GetComponentsInChildren<Rigidbody2D>();
        foreach (var r in limbRigidBodies) r.bodyType = RigidbodyType2D.Dynamic;

        var limbSolvers = DexterModel.GetComponentsInChildren<LimbSolver2D>();
        foreach (var s in limbSolvers) s.enabled = false;
    }

    /// <summary>
    /// Can be used as a coroutine. Stops dexter from flimbo
    /// </summary>
    /// <param name="lerp"></param>
    /// <returns></returns>
    private void StopRagdolling(YoinkMode mode = YoinkMode.Off)
    {
        var dexterLoc = DexterModel.GetComponent<Transform>();
        var parentLoc = GetComponent<Transform>();

        // Enable normal dexter collider
        collider.isTrigger = false;
        rb2d.isKinematic = false;

        parentLoc.position = dexterLoc.position;
        rb2d.position = dexterLoc.position;
        dexterLoc.localPosition = Vector3.zero;

        // Disable all of the garbage
        var hingeJoints = DexterModel.GetComponentsInChildren<HingeJoint2D>();
        foreach (var j in hingeJoints) j.enabled = false;

        var limbColliders = DexterModel.GetComponentsInChildren<EdgeCollider2D>();
        foreach (var l in limbColliders) l.isTrigger = true;

        var limbRigidBodies = DexterModel.GetComponentsInChildren<Rigidbody2D>();
        foreach (var r in limbRigidBodies) r.bodyType = RigidbodyType2D.Kinematic;

        switch (mode)
        {
            case YoinkMode.Teleport:
                LoadBoneLocations();
                break;
            case YoinkMode.Lerp:
                YoinkBoneLocations();
                StartCoroutine(WaitForYoink());
                return;
            case YoinkMode.Off:
                break;
        }

        YoinkFinisher();

    }

    private void YoinkFinisher()
    {
        var limbSolvers = DexterModel.GetComponentsInChildren<LimbSolver2D>();
        foreach (var s in limbSolvers) s.enabled = true;

        ragdolling = false;
        animator.SetBool("Ragdoll", false);
        canGetMagneted = false; 
    }

    private IEnumerator WaitForYoink()
    {
        yield return new WaitForSeconds(yoinkLength);
        YoinkFinisher();
    }

    /// <summary>
    /// Put dexter back together again INSTANTLY
    /// </summary>
    private void LoadBoneLocations()
    {
        foreach (var bone in DexterModel.GetComponentsInChildren<Transform>())
        {
            if (standingDexter.ContainsKey(bone.gameObject.name))
            {
                var loc = standingDexter[bone.gameObject.name];
                bone.localPosition = loc.Item1;
                bone.localRotation = loc.Item2;
            }
        }
    }


    /// <summary>
    /// Try to lerp dexter back together
    /// </summary>
    private void YoinkBoneLocations()
    {
        foreach (var bone in DexterModel.GetComponentsInChildren<Transform>())
        {
            if (standingDexter.ContainsKey(bone.gameObject.name))
            {
                var loc = standingDexter[bone.gameObject.name];
                var yoink = bone.gameObject.AddComponent<BootYoinker>();
                yoink.start = (bone.localPosition, bone.localRotation);
                yoink.end = loc;
                yoink.length = yoinkLength;
            }
        }
    }

    #endregion
}