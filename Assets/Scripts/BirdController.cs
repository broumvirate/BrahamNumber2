using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    private PlayerController c;

    // Start is called before the first frame update
    void Awake()
    {
        c = FindObjectOfType<PlayerController>();
    }
}
