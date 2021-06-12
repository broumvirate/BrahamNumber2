using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGrounded : MonoBehaviour
{
    public LayerMask groundLayer;
    public bool grounded;
    private float distToGround;
    // Start is called before the first frame update
    void Start()
    {
        distToGround = GetComponent<BoxCollider2D>().bounds.extents.y;
    }
    //check if the object is on the ground by detecting a raycast hit below it
    private void Update()
    {
        RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, Vector2.down, distToGround + 0.1f, groundLayer);
        
        //Debug.Log(groundCheck.collider.gameObject.name);

        if(groundCheck.collider != null)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }
}
