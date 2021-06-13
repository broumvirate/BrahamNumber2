using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splodyBarrel : MonoBehaviour
{
    public float area;
    public float force;
    public LayerMask LayerToHit;

    public GameObject ExplodeFX;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<MakeLethal>() != null)
        {
            explode();
        }
    }

    private void explode()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, area, LayerToHit);

        foreach (Collider2D obj in objects)
        {
            Vector2 direction = obj.transform.position - transform.position;

            obj.GetComponent<Rigidbody2D>().AddForce(direction * force);
        }

        GameObject ExplodeFXIns = Instantiate(ExplodeFX, transform.position, Quaternion.identity);
        Destroy(ExplodeFXIns, 10);
    }
}
