using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Birderson : MonoBehaviour
{
    public BoxCollider2D boxEnter;
    public GameObject birdWithChain;
    private BirdMovement bird;
    private CameraControls cam;

    private bool hasBeenTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        bird = birdWithChain.GetComponentInChildren<BirdMovement>();
        bird.GetComponent<Rigidbody2D>().gravityScale = 0f;
        bird.canMove = false;
        cam = FindObjectOfType<CameraControls>();
        cam.birdFucker = cam.dexter;
    }

    // Update is called once per frame
    void Update()
    {
        if(cam.transform.position.x > 10 && !hasBeenTriggered)
        {
            hasBeenTriggered = true;
            StartCoroutine(EnableBird());
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<BoxCollider2D>() == boxEnter && !hasBeenTriggered)
        {
            hasBeenTriggered = true;
            StartCoroutine(EnableBird());
        }
        else if (col.GetComponent<Tutorial_Popup>() != null)
        {
            col.GetComponent<Tutorial_Popup>().Pop();
        }
    }

    public IEnumerator EnableBird()
    {
        bird.canMove = true;
        bird.GetComponent<Rigidbody2D>().gravityScale = 0.7f;
        cam.birdFucker = birdWithChain.GetComponentInChildren<BirdMovement>().gameObject;
        yield return null;
    }

}
