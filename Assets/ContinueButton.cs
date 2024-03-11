using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    private void Start()
    {
        string path = Application.persistentDataPath + "/Save.json";
        SaveData data = JsonUtility.FromJson<SaveData>(System.IO.File.ReadAllText(path));
        if (!data.IsDefaultSave)
        {
            GetComponent<UnityEngine.UI.Button>().colors = UnityEngine.UI.ColorBlock.defaultColorBlock;
        }
    }
}
