using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffect : MonoBehaviour
{
    public float TimeBeforeDestroy;
    void Start()
    {
        Invoke("Delete", TimeBeforeDestroy);
    }
    public void Delete()
    {
        Destroy(gameObject);
    }
}
