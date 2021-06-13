using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hookBehavior : MonoBehaviour
{
    public KeyCode ReleaseKey = KeyCode.M;
    public GameObject magnet;
    public GameObject dexterParent;

    private FixedJoint2D activeJoint = null;
    private float detectionRadius = 0.2f;
    private bool attached;
    // Start is called before the first frame update
    void Start()
    {
        attached = false;
        //Quaternion dexterRotation = dexterParent.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 toMagnet = magnet.transform.position - transform.position;
        float dist = toMagnet.magnitude;
        if (dexterParent.GetComponent<DexterMovement>().reaching && dist <= detectionRadius && !attached)
        {
            attachToMagnet();
        }
        if (Input.GetKeyDown(ReleaseKey) && attached)
        {
            detachFromMagnet();
        }
    }

    private void attachToMagnet()
    {
        activeJoint = gameObject.AddComponent<FixedJoint2D>();
        activeJoint.connectedBody = dexterParent.GetComponent<Rigidbody2D>();
        activeJoint.anchor = magnet.transform.position;
        activeJoint.connectedAnchor = magnet.transform.position;
        attached = true;
        dexterParent.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        dexterParent.GetComponent<DexterMovement>().enabled = false;
        dexterParent.GetComponent<DexterMovement>().hooked = true;
    }

    private void detachFromMagnet()
    {
        if(activeJoint != null)
        {
            attached = false;
            Destroy(activeJoint);
            activeJoint = null;
            dexterParent.GetComponent<DexterMovement>().enabled = true;
            dexterParent.GetComponent<DexterMovement>().hooked = false;
        }
    }
}
