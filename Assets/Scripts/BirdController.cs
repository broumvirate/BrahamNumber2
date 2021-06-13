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

    public void Kill()
    {
        StartCoroutine(Kill2());
    }

    public IEnumerator Kill2()
    {
        // do thing
        yield return new WaitForSeconds(1.5f);
        c.RestartScene();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<MakeLethal>() != null)
        {
            Kill();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<MakeLethal>() != null)
        {
            Kill();
        }
    }
}
