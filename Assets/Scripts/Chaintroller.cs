using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Chaintroller : MonoBehaviour
{
    private Rigidbody2D birdRb;
    private Collider2D birdCollider2D;

    public DexterMovement dexter;
    public GameObject birdModel;
    public List<GameObject> chains;
    public GameObject magnet;

    // Start is called before the first frame update
    void Start()
    {
        birdRb = birdModel.GetComponent<Rigidbody2D>();
        birdCollider2D = birdModel.GetComponent<BoxCollider2D>();
        var dexterCollider = dexter.GetComponent<Collider2D>();
        foreach (var chain in chains)
        {
            Physics2D.IgnoreCollision(chain.GetComponent<Collider2D>(), birdCollider2D);
            Physics2D.IgnoreCollision(chain.GetComponent<Collider2D>(), dexterCollider);
        }
        Physics2D.IgnoreCollision(magnet.GetComponent<PolygonCollider2D>(), birdCollider2D);
        Physics2D.IgnoreCollision(magnet.GetComponent<PolygonCollider2D>(), dexterCollider);
        Physics2D.IgnoreCollision(birdCollider2D, dexterCollider);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
