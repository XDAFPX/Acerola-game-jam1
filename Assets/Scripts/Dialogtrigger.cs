using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dialogtrigger : MonoBehaviour
{
    public bool RequeredInputToStart = true;
    [HideInInspector] public bool IsInReach;
    public InputActionAsset actions;
    public string[] Senteces;
    public string Name;
    public float TimePerCharacter;
    private bool IsStarted;
    private void Start()
    {
        actions.FindActionMap("GunPlay").FindAction("Interact").performed += OnInteract;
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if(DialogUI.Singleton.IsinDialog == this)
            DialogUI.Singleton.Interact(this);
        else if (IsInReach)
        {
            if (DialogUI.Singleton.IsinDialog != null) { DialogUI.Singleton.StopDialog(); }
            DialogUI.Singleton.Interact(this);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<Player>())
        {
            IsInReach = true;
        }
        if (!RequeredInputToStart && !IsStarted)
        {
            if (DialogUI.Singleton.IsinDialog != null) { DialogUI.Singleton.StopDialog(); }
            DialogUI.Singleton.Interact(this);
            IsStarted = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<Player>())
        {
                IsInReach = false;
        }
    }
}
