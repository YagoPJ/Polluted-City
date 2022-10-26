using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioGame : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("musicGameVolume")) // se nao existir um save, ele bota a musica em 1 e carrega dados
        {
            PlayerPrefs.SetFloat("musicGameVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("musicGameVolume", volumeSlider.value);
    }

    public void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicGameVolume");
    }
}
