using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject BlackOutSquare;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FadeToBlack(bool blackOut = true, float fadeSpeed = 1.5f)
    {
        Color color = BlackOutSquare.GetComponent<Image>().color;
        float fadeAmount;

        if (blackOut)
        {
            while (BlackOutSquare.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = color.a + (fadeSpeed * Time.deltaTime);
                color = new Color(color.r, color.g, color.b, fadeAmount);
                BlackOutSquare.GetComponent<Image>().color = color;
                yield return null;
            }
        }
        else
        {
            while (BlackOutSquare.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = color.a - (fadeSpeed * Time.deltaTime);
                color = new Color(color.r, color.g, color.b, fadeAmount);
                BlackOutSquare.GetComponent<Image>().color = color;
                yield return null;
            }
        }

    }
}
