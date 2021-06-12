using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BootYoinker : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform start;
    public Transform end;

    private Transform me;
    public float length;

    private float startTime;
    void Start()
    {
        me = GetComponent<Transform>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var progress = (Time.time - startTime) / length;
        if (progress > 1)
        {
            Destroy(this);
        }
        else
        {
            if (progress > 0.3)
            {
                Debugger.Break();
            }
            me.localPosition = Vector3.Lerp(start.localPosition, end.localPosition, progress);
            me.localRotation = Quaternion.Lerp(start.localRotation, end.localRotation, progress);
        }
    }
}
