using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffect : MonoBehaviour
{
    public float TimeBeforeDestroy;
    void Start()
    {
        Destroy(gameObject, TimeBeforeDestroy);
    }

}
