using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public LayerMask groundLayer;
    public GameObject dexter;
    public GameObject birdFucker;
    public Camera cam;
    public float minZoom = 6.0f;
    public float maxZoom = 15.0f;
    public float zoomLimiter = 16/9;
    public float distMultiplier;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currPos = transform.position;
        Vector3 desiredPos = dexter.transform.position;
        desiredPos.x += 4.0f;
        desiredPos.y += 2.0f;
        desiredPos.z = -10.0f;
        desiredPos = Vector3.Lerp(currPos, desiredPos, 0.1f);

        List<Vector2> focalPoints = new List<Vector2> { };
        focalPoints.Add(dexter.transform.position);
        focalPoints.Add(birdFucker.transform.position);

        //raycast down to detect ground level beneath camera, then zoom out to accommodate it
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, groundLayer);
        if (hit.collider != null)
        {
            focalPoints.Add(hit.point);
        }
        
        //zoom to acommodate dexter, platform, and Bird
        Zoom(focalPoints);
        
        
    }

    void Zoom(List<Vector2> inputList)
    {

        //lemme explain
        //initialize variables for focal point position averaging
        /*float xAvg = 0;
        float yAvg = 0;*/
        //average all the x and y values in the input list
        /*for(int i = 0; i < inputList.Count; i++)
        {
            xAvg += inputList[i].x;
            yAvg += inputList[i].y;
        }
        xAvg /= inputList.Count;
        yAvg /= inputList.Count;*/

        //determine whether the character is moving right or left, so we can put the camera ahead of the direction of movement
        int xTrajectory = 1;
        float dexXVel = dexter.GetComponent<Rigidbody2D>().velocity.x;
        if(dexXVel < 0 && Mathf.Abs(dexXVel) > 0.1)
        {
            xTrajectory = -1;
        }
        else if(dexXVel > 0 && Mathf.Abs(dexXVel) > 0.1)
        {
            xTrajectory = 1;
        }
        //lerp camera position to the average of all values, with constant modifiers (2 units ahead of motion in x, static 3 units above of y), 
        transform.position = Vector3.Lerp(transform.position, new Vector3(dexter.transform.position.x + (2.0f * xTrajectory), dexter.transform.position.y + 1.0f, -10), Time.deltaTime * 2);
        //Debug.Log("theoretically changing position");
        //float newZoom = Mathf.Lerp(maxZoom, minZoom, getGreatestDistance(inputList)/zoomLimiter);
        float newZoom = getGreatestDistance(inputList);
        if(newZoom < minZoom)
        {
            newZoom = minZoom;
        }
        else if(newZoom > maxZoom)
        {
            newZoom = maxZoom;
        }
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }

    float getGreatestDistance(List<Vector2> inputList)
    {
        float greatest = 0;
        for(int i = 0; i < inputList.Count; i++)
        {
            Vector2 distanceVector = new Vector2(transform.position.x, transform.position.y) - inputList[i];
            float dist = distanceVector.magnitude;
            if (dist > greatest)
            {
                greatest = dist;
            }
        }
        return greatest * distMultiplier;
    }
}
