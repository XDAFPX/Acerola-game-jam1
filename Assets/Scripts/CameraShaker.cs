using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker Singleton;
    public CinemachineVirtualCamera cam;
    private void Awake()
    {
        Singleton = this;
    }
    private void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }
    public void StartShake(float Amplitude,float frequincy)
    {
        var noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = Amplitude;
        noise.m_FrequencyGain = frequincy;
    }
    public void StartShake(float Amplitude, float frequincy,float lifespan)
    {
        var noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = Amplitude;
        noise.m_FrequencyGain = frequincy;
        Invoke(nameof(StopShake), lifespan);
    }
    public void StopShake()
    {
        var noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }
}
