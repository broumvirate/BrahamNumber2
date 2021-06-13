using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beatroch : MonoBehaviour
{
    private PlayerController pc;
    // Start is called before the first frame update
    void Start()
    {
        pc = FindObjectOfType<PlayerController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<DexterMovement>() != null)
        {
            StartCoroutine(pc.Win());
        }
    }
}
