using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeTrigger : MonoBehaviour
{
    public float Amplitude;
    public float Frequincy;
    public void StartShake()
    {
        CameraShaker.Singleton.StartShake(Amplitude, Frequincy);
    }
    public void EndShake()
    {
        CameraShaker.Singleton.StopShake();
    }
}
