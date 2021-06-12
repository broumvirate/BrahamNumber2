using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeLethal : MonoBehaviour
{
    private PlayerController c;


    void Awake()
    {
        c = FindObjectOfType<PlayerController>();
        Debug.Log(c.gameObject.name);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject == c.gameObject)
        {
            c.kill();
        }
    }
}
