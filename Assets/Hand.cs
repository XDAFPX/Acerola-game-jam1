using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public void Play()
    {
        GetComponent<AudioSource>().Play();
    }
}
