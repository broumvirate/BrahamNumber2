using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public void loadLevel(int level)
    {
        //sets PlayerPrefs
        if (level == 1)
        {
            PlayerPrefs.SetFloat("saveX", -23.32631f);
            PlayerPrefs.SetFloat("saveY", -2.25f);
            PlayerPrefs.SetFloat("saveZ", 3.502673f);
            PlayerPrefs.SetInt("hasBird", 0);
        }

        Debug.Log(PlayerPrefs.GetInt("hasBird"));
        SceneManager.LoadScene(level);

    }
}
