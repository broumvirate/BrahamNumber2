using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SecretTiles: MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null || collision.gameObject.GetComponent<BirdController>() != null)
        {
            gameObject.GetComponent<TilemapRenderer>().enabled = false;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null || collision.gameObject.GetComponent<BirdController>() != null)
        {
            gameObject.GetComponent<TilemapRenderer>().enabled = true;
        }
    }
}
