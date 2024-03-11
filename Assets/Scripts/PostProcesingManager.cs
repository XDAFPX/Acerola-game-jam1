using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcesingManager : MonoBehaviour
{
    public static PostProcesingManager Singleton;
    private Volume GlobalVolume;
    private Volume SpecialVolume;
    private Volume DamagedVolume;
    public bool IsTriggered = false;
    private void Awake()
    {
        Singleton = this;
    }
    private void Start()
    {
        GlobalVolume = GetComponent<Volume>();
        SpecialVolume = transform.GetChild(0).GetComponent<Volume>();
        DamagedVolume = transform.GetChild(1).GetComponent<Volume>();
    }

    private void FixedUpdate()
    {
        if (IsTriggered)
        {
            GlobalVolume.weight = Mathf.Clamp(GlobalVolume.weight - 0.005f, 0, 1);
            SpecialVolume.weight = Mathf.Clamp(SpecialVolume.weight + 0.005f, 0, 1);
        }
    }
    public void SetDamagedPost(float value)
    {
        DamagedVolume.weight = value;
    }
}
