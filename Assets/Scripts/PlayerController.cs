using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public bool isDead = false;
    private UIController uiController;

    void Start()
    {
        uiController = FindObjectOfType<UIController>();
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
        yield return new WaitForSeconds(1f);
        yield return RestartScene();

    }

    public IEnumerator RestartScene()
    {
        yield return uiController.FadeToBlack(true, 3);
        yield return new WaitForSeconds(0.3f);

        yield return uiController.FadeToBlack(false, 10);
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
