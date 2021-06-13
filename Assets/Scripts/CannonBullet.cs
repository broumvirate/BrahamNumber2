using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBullet : MonoBehaviour
{
    private Vector2 startingLocation;
    [HideInInspector]
    public float distance;
    // Start is called before the first frame update
    void Start()
    {
        startingLocation = GetComponent<Transform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        var currentDistance = Vector2.Distance(startingLocation, GetComponent<Transform>().position);
        {
            if (currentDistance >= distance)
            {
                Destroy(gameObject);
            }
        }
    }
}
