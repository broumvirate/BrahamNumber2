using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hookBehavior : MonoBehaviour
{
    public KeyCode ReleaseKey = KeyCode.Space;
    public GameObject magnet;
    public GameObject dexterParent;

    private FixedJoint2D activeJoint = null;
    private float detectionRadius = 0.2f;
    public bool attached;
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
        if (!attached)
        {
            activeJoint = dexterParent.AddComponent<FixedJoint2D>();
            activeJoint.connectedBody = magnet.GetComponent<Rigidbody2D>();
            activeJoint.anchor = magnet.transform.position;
            activeJoint.connectedAnchor = magnet.transform.position;
            attached = true;
            dexterParent.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            dexterParent.GetComponent<DexterMovement>().enabled = false;
            dexterParent.GetComponent<DexterMovement>().hooked = true;
        }
    }

    private void detachFromMagnet()
    {
        if(activeJoint != null && attached)
        {
            Destroy(activeJoint);
            activeJoint = null;
            dexterParent.GetComponent<DexterMovement>().enabled = true;
            dexterParent.GetComponent<DexterMovement>().hooked = false;
            dexterParent.GetComponent<DexterMovement>().recoveryPeriod = true;
            StartCoroutine(bricksterCooldown());
        }
    }

    private IEnumerator bricksterCooldown()
    {
        //Debug.Log("Running brickster cooldown");
        yield return new WaitForSeconds(0.5f);
        //Debug.Log("Setting Attached to False");
        attached = false;
    }
}
