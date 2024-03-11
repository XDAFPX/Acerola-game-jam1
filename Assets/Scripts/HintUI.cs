using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintUI : MonoBehaviour
{
    public static HintUI Singleton;
    private void Awake()
    {
        Singleton = this;
    }
    public void ShowHint(string text)
    {
        GetComponent<Animator>().SetBool("IsShown", true);
        transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = text;
        Invoke(nameof(HideHint), 3);
    }
    public void HideHint()
    {
        GetComponent<Animator>().SetBool("IsShown", false);
    }
}
