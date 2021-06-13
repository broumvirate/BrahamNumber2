using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lvl1boulder : MonoBehaviour
{
    private Vector3 initPosit;
    // Start is called before the first frame update
    void Start()
    {
        initPosit = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10)
        {
            transform.position = initPosit;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
    }
}
