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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseSettings();
        }
    }
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
        Master.GetFloat("sfx", out float value1);
        Master.GetFloat("music", out float value2);
        //Sliders[1].value = Mathf.Pow(10,value1)/20;
        //Sliders[0].value = Mathf.Pow(10, value2) / 20;
    }
    public void CloseSettings()
    {
        Settings.SetActive(false);
    }
}
