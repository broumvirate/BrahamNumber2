using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] GameObject dexter;
    [SerializeField] GameObject bird;
    [SerializeField] GameObject bController;

    public void Start()
    {
        dexter = FindObjectOfType<PlayerController>().gameObject;
        bird = FindObjectOfType<BirdController>().gameObject;
        bController = FindObjectOfType<Chaintroller>().gameObject;
        Debug.Log(bird);

        if (PlayerPrefs.HasKey("saveX") is false)
        {
            PlayerPrefs.SetFloat("saveX", dexter.transform.position.x);
            PlayerPrefs.SetFloat("saveY", dexter.transform.position.y);
            PlayerPrefs.SetFloat("saveZ", dexter.transform.position.z);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == dexter)
        {
            dexter.transform.position = new Vector3(PlayerPrefs.GetFloat("saveX"), PlayerPrefs.GetFloat("saveY"), PlayerPrefs.GetFloat("saveZ"));

            //calculates target Bird position
            if (PlayerPrefs.GetInt("hasBird") == 1)
            {
                Vector3 store = bird.transform.position - bController.transform.position;
                //Debug.Log(bController.name);
                //Debug.Log(PlayerPrefs.GetFloat("saveY") - store.y);
                bController.transform.position = new Vector3(PlayerPrefs.GetFloat("saveX") - store.x, PlayerPrefs.GetFloat("saveY") - store.y, PlayerPrefs.GetFloat("saveZ") - store.z);
                bird.GetComponentInChildren<BirdMovement>().canMove = true;
            }
        }
    }
}
