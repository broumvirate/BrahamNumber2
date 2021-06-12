using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    #region Variables

    private Rigidbody2D rb;

    public float speed;
    public float max_speed;
    public float jumpVelocity;

    protected bool isOnGround
    {
        get;
        private set;
    }
    
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
    }
}
