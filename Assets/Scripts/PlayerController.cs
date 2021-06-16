using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public AudioClip dexterDeath;
    public AudioSource audio;
    public bool isDead = false;
    private UIController uiController;

    public bool canSave;

    void Start()
    {
        uiController = FindObjectOfType<UIController>();
    }

    /// <summary>
    /// Kills dexter
    /// </summary>
    public void Kill()
    {
        StartCoroutine(Kill2());
    }

    /// <summary>
    /// Also kills dexter
    /// </summary>
    /// <returns></returns>
    public IEnumerator Kill2()
    {
        GetComponent<Ragdoll>().StartRagdolling();
        var spine = GetComponentsInChildren<Rigidbody2D>().First(t => t.gameObject.name == "Spine");
        spine.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        audio.PlayOneShot(dexterDeath, 0.7F);
        yield return new WaitForSeconds(1f);
        yield return RestartScene();
        Destroy(gameObject);

    }

    /// <summary>
    /// CALL THIS WHEN YOU DIE TO RESTART EVERYTHING
    /// </summary>
    /// <returns></returns>
    public IEnumerator RestartScene()
    {
        yield return uiController.FadeToBlack(true, 3);
        yield return new WaitForSeconds(0.3f);

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);

        // TODO: Make this fucking work
        //yield return uiController.FadeToBlack(false, 10);

    }

    public IEnumerator Win()
    {
        yield return uiController.FadeToWin(true, 3);
        //Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(3f);

        SceneManager.LoadScene("MainMenu");
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

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && canSave)
        {
            PlayerPrefs.SetFloat("saveX", gameObject.transform.position.x);
            PlayerPrefs.SetFloat("saveY", gameObject.transform.position.y);
            PlayerPrefs.SetFloat("saveZ", gameObject.transform.position.z);
            Debug.Log("Saved!");
        }

    }*/
}
