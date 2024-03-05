using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ItemAsset", order = 1)]
public class Item : ScriptableObject
{
    public string Name;
    public string OnUseMethodName;
    public string OnPickupMethodName;
    public Sprite InventoryIcon;
    public Sprite WorldIcon;
    public Vector2 WorldSize;
    public Vector2 UISize;
    public Vector2 TriggerSize;
    public bool CollectItemOnPickup;
}
