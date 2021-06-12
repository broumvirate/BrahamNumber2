using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    #region Variables

    private Rigidbody2D rb;
    private Collider2D birdCollider2D;

    public float speed;
    public float max_speed;
    public float jumpVelocity;

    public List<GameObject> chains = new List<GameObject>(5);
    public Magnet magnet;
    public GameObject birdModel;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = birdModel.GetComponent<Rigidbody2D>();
        birdCollider2D = birdModel.GetComponent<Collider2D>();
        foreach (var chain in chains)
        {
            Physics2D.IgnoreCollision(chain.GetComponent<Collider2D>(), birdCollider2D);
        }
        Physics2D.IgnoreCollision(birdCollider2D, magnet.GetComponent<BoxCollider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
    }

    void Move()
    {
        if (GetComponent<CheckGrounded>().grounded == false)
        {
            float x = Input.GetAxis("Horizontal");
            float moveBy = x * speed;
            if (moveBy != 0f)
            {
                rb.velocity = new Vector2(moveBy, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x * 0.7f, rb.velocity.y);
            }
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
    }
}
