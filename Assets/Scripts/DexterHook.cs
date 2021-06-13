using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DexterHook : MonoBehaviour, IMagnetic
{
    public DexterMovement Dexter;
    public float strength = 10f;
    [HideInInspector]
    public bool isMagnetized;
    private GameObject magnet;
    private FixedJoint2D magnetFixedJoint2D;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMagnetized)
        {
            MoveTowardsMagnet();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // When entering magnet range
        var target = collider.GetComponent<Magnet>();
        if (target != null && Dexter.GetComponent<Ragdoll>().ragdolling)
        {
            target.magnetizedList.Add(this);
            isMagnetized = true;
            this.magnet = collider.gameObject;
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject == this.magnet)
        {
            isMagnetized = false;
            magnet = null;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Creates fixedjoint2d when touching the actual magnet
        if (collision.gameObject == this.magnet && Dexter.canGetMagneted && Dexter.GetComponent<Ragdoll>().ragdolling)
        {
            isMagnetized = false;
            var joint = gameObject.AddComponent<FixedJoint2D>();
            joint.connectedBody = magnet.GetComponent<Rigidbody2D>();
            magnetFixedJoint2D = joint;
        }
    }

    public void MoveTowardsMagnet()
    {
        var target = magnet.GetComponent<Rigidbody2D>().position;
        var distance = Vector2.Distance(transform.position, target);
        var maxDistance = (1 / distance) * strength;
        transform.position = Vector2.MoveTowards(transform.position, target, maxDistance * Time.deltaTime);
    }

    public void FreeFromMagnet(Magnet m)
    {
        isMagnetized = false;
        Destroy(magnetFixedJoint2D);
        Dexter.canGetMagneted = false;
        StartCoroutine("FreeFromMagnet2");

    }

    private IEnumerator FreeFromMagnet2()
    {
        yield return new WaitForSeconds(0.3f);
        isMagnetized = true;
    }


}
