using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public bool isDead = false;
    private DexterMovement dexter;

    void Start()
    {
        dexter = GetComponent<DexterMovement>();
    }

    public void Kill()
    {
        StartCoroutine(Kill2());
    }

    public IEnumerator Kill2()
    {
        GetComponent<Ragdoll>().StartRagdolling();
        var spine = GetComponentsInChildren<Rigidbody2D>().First(t => t.gameObject.name == "Spine");
        spine.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        yield return new WaitForSeconds(2f);
        RestartScene();
    }

    public void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
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
