using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public AudioMixer Master;
    public Slider[] Sliders;
    public GameObject Settings;
    public void OnChanged()
    {
        
        if (Sliders[1])
            Master.SetFloat("sfx", Mathf.Log10(Sliders[1].value) * 20);
        if (Sliders[0])
            Master.SetFloat("music", Mathf.Log10(Sliders[0].value) * 20);
        
    }
    public void OpenSettings()
    {
        Settings.SetActive(true);
    }
    public void CloseSettings()
    {
        Settings.SetActive(false);
    }
}
