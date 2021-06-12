using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public List<IMagnetic> magnetizedList;

    public KeyCode ReleaseKey = KeyCode.M;

    // Start is called before the first frame update
    void Start()
    {
        magnetizedList = new List<IMagnetic>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(ReleaseKey))
        {
            MagnetRelease();
        }
    }

    /// <summary>
    /// Calls the FreeFromMagnet method on all of the things we've magnetized
    /// </summary>
    void MagnetRelease()
    {
        foreach (var obj in magnetizedList)
        {
            obj.FreeFromMagnet(this);
        }
    }
}
