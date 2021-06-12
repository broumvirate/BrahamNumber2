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
        //dont look at this part or i'll kill you
        //RaycastHit2D groundCheck = Physics2D.CapsuleCast(GetComponent<BoxCollider2D>().bounds.center, new Vector2(GetComponent<BoxCollider2D>().bounds.center.x, GetComponent<BoxCollider2D>().bounds.min.y - 0.1f) , CapsuleDirection2D.Vertical, 180.0f, Vector2.down, 0.1f, groundLayer);
        //rather than use a capsule collider (i dunno how to lol) i'm using two raycasts, sourced from either side of the sprite.
        RaycastHit2D groundCheck = Physics2D.Raycast(new Vector2(transform.position.x - (GetComponent<BoxCollider2D>().bounds.extents.x - 0.1f), transform.position.y), Vector2.down, distToGround + 0.1f, groundLayer);
        RaycastHit2D groundCheck2 = Physics2D.Raycast(new Vector2(transform.position.x + (GetComponent<BoxCollider2D>().bounds.extents.x - 0.1f), transform.position.y), Vector2.down, distToGround + 0.1f, groundLayer);
        //Debug.Log(groundCheck.collider.gameObject.name);

        if (groundCheck.collider != null || groundCheck2.collider != null)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }
}
