using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Popup : MonoBehaviour
{
    public GameObject popper;
    public float popLength;
    private float startTime;
    private bool isPopping = false;
    // Crap, a pop up!

    // Start is called before the first frame update
    void Start()
    {
        popper.transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPopping)
        {
            var progress = (Time.time - startTime) / popLength;
            if (popLength > 1)
            {
                isPopping = false;
            }
            else
            {
                popper.transform.localScale = Vector3.one * progress;
            }
        }
    }

    void Pop()
    {
        startTime = Time.time;
        isPopping = true;
    }
}
