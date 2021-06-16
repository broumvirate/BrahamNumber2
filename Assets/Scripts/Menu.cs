using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Continue()
    {
        PlayerPrefs.SetFloat("saveX", -23.32631f);
        PlayerPrefs.SetFloat("saveY", -2.25f);
        PlayerPrefs.SetFloat("saveZ", 3.502673f);
        PlayerPrefs.SetInt("hasBird", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
