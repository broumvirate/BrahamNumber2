using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Birderson : MonoBehaviour
{
    public BoxCollider2D boxEnter;
    public GameObject birdWithChain;
    private CameraControls cam;

    private bool hasBeenTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        birdWithChain.SetActive(false);
        cam = FindObjectOfType<CameraControls>();
        cam.birdFucker = cam.dexter;
    }

    // Update is called once per frame
    void Update()
    {

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
        birdWithChain.SetActive(true);
        cam.birdFucker = birdWithChain.GetComponentInChildren<BirdMovement>().gameObject;
        yield return null;
    }

}
