using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public abstract class UIElement : MonoBehaviour
{
    public UIBehaviour[] elements;
    public bool RequireCursor = true;
    public bool IsOpened;

    public virtual void OpenElement()
    {
        foreach (var item in elements)
        {
            item.enabled = true;
        }
        if (RequireCursor) Player.ShowCursor = true;
        IsOpened = true;
    }
    public virtual void CloseElement()
    {
        foreach (var item in elements)
        {
            item.enabled = false;
        }
        if (RequireCursor) Player.ShowCursor = false;
        IsOpened = false;
    }
}
