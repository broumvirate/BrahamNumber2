
using UnityEngine;

public class DexterMovement : MonoBehaviour
{
    public KeyCode jump = KeyCode.W;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode moveLeft = KeyCode.A;
    public float horizSpeed = 3.0f;
    public float jumpPower = 6.0f;
    public bool ragdolling;
    float distToGround;
    //public float boundY = 2.25f; PUT THIS BACK IN TO LIMIT FROM FALLING OFF-SCREEN
    private Rigidbody2D rb2d;
    // Use this for initialization
    void Start()
    {
        ragdolling = false;
        rb2d = GetComponent<Rigidbody2D>();
        distToGround = GetComponent<Collider2D>().bounds.extents.y + 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vel = rb2d.velocity;//velocity is set to current velocity
        if (!ragdolling)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            if (Input.GetKey(jump))
            {
                //if touching ground:
                if (GetComponent<CheckGrounded>().grounded)
                {
                    vel.y = jumpPower; //placeholder jump height value
                }
            }
            if (Input.GetKey(moveRight))
            {
                //RaycastHit2D wallCheck = Physics2D.Raycast(new Vector2(transform.position.x - (GetComponent<BoxCollider2D>().bounds.extents.x - 0.1f), transform.position.y), Vector2.down, distToGround + 0.1f, groundLayer);
                if (vel.x <= horizSpeed)
                {
                    vel.x = vel.x + 1;
                }
            }
            else if (Input.GetKey(moveLeft))
            {
                if (vel.x >= -horizSpeed)
                {
                    vel.x = vel.x - 1;
                }
            }
            else
            {
                vel.x /= 2;
            }

            if (GetComponent<CheckGrounded>().grounded && vel.y < 0)
            {
                vel.y = 0;

                rb2d.velocity = vel;
            }
            else
            {
                vel.y -= 0.01f;
            }

            rb2d.velocity = vel;
            Debug.Log(vel);
        }
        

        /*var pos = transform.position;
		if (pos.y > boundY)
		{
			pos.y = boundY;
		}
		else if (pos.y < -boundY)
		{
			pos.y = -boundY;
		}
		transform.position = pos;*/

    }
}