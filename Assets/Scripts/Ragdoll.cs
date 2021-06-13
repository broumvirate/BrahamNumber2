using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.IK;

public enum YoinkMode
{
    Off,
    Teleport,
    Lerp
}

public class Ragdoll : MonoBehaviour
{
    public GameObject Model;
    private Dictionary<string, (Vector3, Quaternion)> initialPosition = new Dictionary<string, (Vector3, Quaternion)>();
    public string ragdollAnimationName = "Ragdoll";

    public float recoverLength = 0.4f;

    private Rigidbody2D rb2d;
    private Collider2D collider;
    private Animator animator;
    
    [HideInInspector]
    public bool ragdolling;

    private Action thingToDo;

    // Start is called before the first frame update
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponentInChildren<Animator>();
        StopRagdolling(YoinkMode.Off);
    }

    void Start()
    {
        SaveBoneLocations();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Ragdoll

    /// <summary>
    /// Sets all of the current bone locations to the StandingDexter dictionary
    /// </summary>
    private void SaveBoneLocations()
    {
        foreach (var bone in Model.GetComponentsInChildren<Transform>())
        {
            if (!initialPosition.ContainsKey(bone.gameObject.name))
            {
                initialPosition.Add(bone.gameObject.name, (bone.localPosition, bone.localRotation));
            }
        }
    }

    /// <summary>
    /// Dexter go wheee
    /// </summary>
    public void StartRagdolling()
    {
        ragdolling = true;
        rb2d.velocity = Vector2.zero;
        animator.SetBool(ragdollAnimationName, true);

        // Disable dexter normal collider
        collider.isTrigger = true;
        rb2d.isKinematic = true;

        // Enable all of the garbage
        var hingeJoints = Model.GetComponentsInChildren<HingeJoint2D>();
        foreach (var j in hingeJoints) j.enabled = true;

        var limbColliders = Model.GetComponentsInChildren<EdgeCollider2D>();
        foreach (var l in limbColliders) l.isTrigger = false;

        var limbRigidBodies = Model.GetComponentsInChildren<Rigidbody2D>();
        foreach (var r in limbRigidBodies) r.bodyType = RigidbodyType2D.Dynamic;

        var limbSolvers = Model.GetComponentsInChildren<LimbSolver2D>();
        foreach (var s in limbSolvers) s.enabled = false;
    }

    /// <summary>
    /// Can be used as a coroutine. Stops dexter from flimbo
    /// </summary>
    /// <param name="lerp"></param>
    /// <returns></returns>
    public void StopRagdolling(YoinkMode mode = YoinkMode.Off, Action thingToDoAfter = null)
    {
        thingToDo = thingToDoAfter;
        var dexterLoc = Model.GetComponent<Transform>();
        var parentLoc = GetComponent<Transform>();

        // Enable normal dexter collider
        collider.isTrigger = false;
        rb2d.isKinematic = false;

        parentLoc.position = dexterLoc.position;
        rb2d.position = dexterLoc.position;
        dexterLoc.localPosition = Vector3.zero;

        // Disable all of the garbage
        var hingeJoints = Model.GetComponentsInChildren<HingeJoint2D>();
        foreach (var j in hingeJoints) j.enabled = false;

        var limbColliders = Model.GetComponentsInChildren<EdgeCollider2D>();
        foreach (var l in limbColliders) l.isTrigger = true;

        var limbRigidBodies = Model.GetComponentsInChildren<Rigidbody2D>();
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
        var limbSolvers = Model.GetComponentsInChildren<LimbSolver2D>();
        foreach (var s in limbSolvers) s.enabled = true;

        ragdolling = false;
        animator.SetBool("Ragdoll", false);
        thingToDo?.Invoke();
    }

    private IEnumerator WaitForYoink()
    {
        yield return new WaitForSeconds(recoverLength);
        YoinkFinisher();
    }

    /// <summary>
    /// Put dexter back together again INSTANTLY
    /// </summary>
    private void LoadBoneLocations()
    {
        foreach (var bone in Model.GetComponentsInChildren<Transform>())
        {
            if (initialPosition.ContainsKey(bone.gameObject.name))
            {
                var loc = initialPosition[bone.gameObject.name];
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
        foreach (var bone in Model.GetComponentsInChildren<Transform>())
        {
            if (initialPosition.ContainsKey(bone.gameObject.name))
            {
                var loc = initialPosition[bone.gameObject.name];
                var yoink = bone.gameObject.AddComponent<BootYoinker>();
                yoink.start = (bone.localPosition, bone.localRotation);
                yoink.end = loc;
                yoink.length = recoverLength;
            }
        }
    }

    #endregion
}
