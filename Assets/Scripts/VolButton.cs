using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolButton : MonoBehaviour
{
    private Image volIcon;
    [SerializeField] Slider volSlider;
    [SerializeField] Sprite mute;
    [SerializeField] Sprite quiet;
    [SerializeField] Sprite medium;
    [SerializeField] Sprite loud;

    public void Start()
    {
        volIcon = GetComponent<Image>();
    }

    public void OnChange()
    {
        if (volSlider.value == 0f)
        {
            volIcon.sprite = mute;
        }

        else if (volSlider.value <= 0.33f)
        {
            volIcon.sprite = quiet;
        }

        else if (volSlider.value <= 0.66f)
        {
            volIcon.sprite = medium;
        }

        else
        {
            volIcon.sprite = loud;
        }
    }
}
