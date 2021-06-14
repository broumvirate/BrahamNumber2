using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeMan : MonoBehaviour
{

    [SerializeField] Slider volSlider;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("musicVolume") is false)
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }

        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volSlider.value;
        Save();
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volSlider.value);
    }

    private void Load()
    {
        volSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }


}
