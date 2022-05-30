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
        //unconventional way of identifying elements but we'll roll with it
        GameObject dexter = FindObjectOfType<PlayerController>().gameObject;
        GameObject bird = FindObjectOfType<BirdController>().gameObject;
        GameObject bController = FindObjectOfType<Chaintroller>().gameObject;

        //ripping this from Emersons code, blame him if it sucks
        //running this on start() allows dexter to resume from his checkpoint post-death when the scene reloads, because playerprefs transcend that stuff
        dexter.transform.position = new Vector3(PlayerPrefs.GetFloat("saveX"), PlayerPrefs.GetFloat("saveY"), PlayerPrefs.GetFloat("saveZ"));

        if (PlayerPrefs.GetInt("hasBird") == 1)
        {
            bController.transform.position = new Vector3(PlayerPrefs.GetFloat("birdX"), PlayerPrefs.GetFloat("birdY"), PlayerPrefs.GetFloat("birdZ"));
            var tutorial = FindObjectOfType<Tutorial_Birderson>();
            if(tutorial != null)
            {
                StartCoroutine(tutorial.EnableBird());
            }
        }

    }

    /// <summary>
    /// Kills dexter
    /// why the fuck does this exist -alden; because other things kill dexter than the collisionenter -ben
    /// you know you can just call the coroutine directly in the oncollisionenter
    /// i could fix it but i wont to make an example of you
    /// </summary>
    public void Kill()
    {
        if (!isDead)
        {
            isDead = true;
            StartCoroutine(Kill2());
        }
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
        if (collision.gameObject.CompareTag("Checkpoint") && canSave)
        {
            SavePosition();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<MakeLethal>() != null)
        {
            Kill();
            
        }
    }

    private void SavePosition()
    {
        if(canSave)
        {
            var bird = FindObjectOfType<BirdController>();
            var birdMove = FindObjectOfType<Chaintroller>();
            PlayerPrefs.SetFloat("saveX", gameObject.transform.position.x);
            PlayerPrefs.SetFloat("saveY", gameObject.transform.position.y);
            PlayerPrefs.SetFloat("saveZ", gameObject.transform.position.z);
            PlayerPrefs.SetFloat("birdX", bird.transform.position.x);
            PlayerPrefs.SetFloat("birdY", bird.transform.position.y);
            PlayerPrefs.SetFloat("birdZ", bird.transform.position.z);
            Debug.Log("Saved!");
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
