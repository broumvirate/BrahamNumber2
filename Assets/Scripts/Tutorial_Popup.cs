using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Popup : MonoBehaviour
{
    public GameObject popper;
    public float popLength;
    private float startTime;
    private bool isPopping = false;
    private bool hasPopped = false;
    // Crap, a pop up!

    // Start is called before the first frame update
    void Start()
    {
        popper.transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPopping && hasPopped == false)
        {
            var progress = (Time.time - startTime) / popLength;
            if (progress > 1)
            {
                isPopping = false;
                hasPopped = true;
            }
            else
            {
                popper.transform.localScale = Vector3.one * progress;
            }
        }
    }

    public void Pop()
    {
        startTime = Time.time;
        isPopping = true;
    }
}
