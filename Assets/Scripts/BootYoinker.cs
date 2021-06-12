using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BootYoinker : MonoBehaviour
{
    // Start is called before the first frame update
    public (Vector3, Quaternion) start;
    public (Vector3, Quaternion) end;

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
            me.localPosition = Vector3.Lerp(start.Item1, end.Item1, progress);
            me.localRotation = Quaternion.Lerp(start.Item2, end.Item2, progress);
        }
    }
}
