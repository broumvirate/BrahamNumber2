using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeLethal : MonoBehaviour
{
    private PlayerController c;
    private BirdController b;


    void Awake()
    {
        c = FindObjectOfType<PlayerController>();
        b = FindObjectOfType<BirdController>();

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == c.gameObject || collision.gameObject == b.gameObject)
        {
            c.kill();
        }
    }
}
