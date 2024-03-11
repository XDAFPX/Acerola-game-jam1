using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerSceneLoad : MonoBehaviour
{
    public void Load(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void TransitionToNextStage(int stage)
    {
        SaveNLoadManager.Singleton.SaveState();
        SaveNLoadManager.Singleton.AdvanceStage(stage);
        SaveNLoadManager.Singleton.Load();
    }
}
