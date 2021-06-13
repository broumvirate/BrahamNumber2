using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AllOptions : MonoBehaviour
{
    [SerializeField] Button vol;
    [SerializeField] Sprite mute;
    [SerializeField] Sprite quiet;
    [SerializeField] Sprite medium;
    [SerializeField] Sprite loud;
    private Image volIcon;

    public void BackToMain()
    {
        SceneManager.LoadScene(0);
    }

    public void Reset()
    {
        FindObjectOfType<PlayerController>().Kill();
    }

    public void Pause()
    {
        if (Time.timeScale != 0)
        {
            Time.timeScale = 0f;
        }

        else
        {
            Time.timeScale = 1;
        }
    }

    public void Sound()
    {
        volIcon = vol.image;

        if (AudioListener.volume < 0.33f)
        {
            AudioListener.volume = 0.33f;
            volIcon.sprite = quiet;
        }

        else if (AudioListener.volume < 0.66f)
        {
            AudioListener.volume = 0.66f;
            volIcon.sprite = medium;
        }

        else if (AudioListener.volume < 1)
        {
            AudioListener.volume = 1;
            volIcon.sprite = loud;
        }

        else
        {
            AudioListener.volume = 0f;
            volIcon.sprite = mute;
        }
    }
}
