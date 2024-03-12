using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerSceneLoad : MonoBehaviour
{
    public void Load(string scene)
    {
        if(scene != "Menu")
            SceneManager.LoadScene(scene);
        else
        {
            if (SaveNLoadManager.Singleton != null) Destroy(SaveNLoadManager.Singleton);
            SceneManager.LoadScene(scene);
        }

    }

    public void TransitionToNextStage(int stage)
    {
        SaveNLoadManager.Singleton.SaveState();
        SaveNLoadManager.Singleton.AdvanceStage(stage);
        SaveNLoadManager.Singleton.Load();
    }
}
