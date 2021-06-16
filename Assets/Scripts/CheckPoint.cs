using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            PlayerPrefs.SetFloat("saveX", gameObject.transform.position.x);
            PlayerPrefs.SetFloat("saveY", gameObject.transform.position.y);
            PlayerPrefs.SetFloat("saveZ", gameObject.transform.position.z);
            if (FindObjectOfType<BirdMovement>().canMove == true)
            {
                PlayerPrefs.SetInt("hasBird", 1);
            }
            else
            {
                PlayerPrefs.SetInt("hasBird", 1);
            }
            Debug.Log("Saved!");
        }
    }
}
