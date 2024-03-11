using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTrigger : MonoBehaviour
{
    public void TriggerHint(string HintText)
    {
        HintUI.Singleton.ShowHint(HintText);
    }
    public void HideHint()
    {
        HintUI.Singleton.HideHint();
    }
}
