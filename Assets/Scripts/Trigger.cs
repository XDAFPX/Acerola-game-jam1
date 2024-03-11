using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public bool IsCollisonBased;
    public bool IsTriggerBased = true;
    public UnityEvent OnEnter;
    public UnityEvent OnExit;
    public bool TriggerOnExit = true;
    public bool TriggerOnEnter = true;
    public bool TriggerOnes;
    public string Triggerebletag;
    private bool IsTriggered = false;

    public void TriggerEneter()
    {
        OnEnter.Invoke();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsCollisonBased && TriggerOnEnter && !IsTriggered && (collision.collider.gameObject.tag == Triggerebletag))
        {
            OnEnter.Invoke();
            if (TriggerOnes && !IsTriggered) IsTriggered = true;
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (IsCollisonBased && TriggerOnExit && !IsTriggered && (collision.collider.gameObject.tag == Triggerebletag))
        {
            OnExit.Invoke();
            if (TriggerOnes && !IsTriggered) IsTriggered = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsTriggerBased && TriggerOnEnter && !IsTriggered && (collision.gameObject.tag == Triggerebletag))
        {
            OnEnter.Invoke();
            if (TriggerOnes && !IsTriggered) IsTriggered = true;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsTriggerBased && TriggerOnExit && !IsTriggered && (collision.gameObject.tag == Triggerebletag))
        {
            OnExit.Invoke();
            if (TriggerOnes && !IsTriggered) IsTriggered = true;
        }
    }

}
