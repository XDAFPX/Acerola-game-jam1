using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public bool IsCollisonBased;
    public bool IsTriggerBased = true;
    public UnityEvent Events;
    public bool TriggerOnExit;
    public bool TriggerOnEnter = true;
    public bool TriggerOnes;
    private bool IsTriggered = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsCollisonBased && TriggerOnEnter && !IsTriggered )
        {
            Events.Invoke();
        }
        if (TriggerOnes&&!IsTriggered) IsTriggered = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (IsCollisonBased && TriggerOnExit && !IsTriggered)
        {
            Events.Invoke();
        }
        if (TriggerOnes && !IsTriggered) IsTriggered = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsTriggerBased && TriggerOnEnter && !IsTriggered)
        {
            Events.Invoke();
        }
        if (TriggerOnes && !IsTriggered) IsTriggered = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsTriggerBased && TriggerOnExit && !IsTriggered)
        {
            Events.Invoke();
        }
        if (TriggerOnes && !IsTriggered) IsTriggered = true;
    }

}
