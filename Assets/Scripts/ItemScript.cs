using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemScript : MonoBehaviour
{
    public Item Data;
    public bool IsInInventory = false;
    public ParticleSystem PickupParticle;
    private bool IsInReach;
    private void Start()
    {
        LoadData();
    }
    public void LoadData()
    {
        if (Data)
        {
            if (!IsInInventory)
            {
                GetComponent<SpriteRenderer>().sprite = Data.WorldIcon;
                transform.localScale = Data.WorldSize;
                GetComponent<BoxCollider2D>().size = Data.TriggerSize;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = Data.InventoryIcon;
                Destroy(gameObject);
            }
        }
    }
    public void TryPickUp()
    {
        if (IsInReach && !IsInInventory)
        {
            Player pl = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            if (PickupParticle)
                Instantiate(PickupParticle, transform.position, Quaternion.identity);
            IsInInventory = true;
            if (Data.CollectItemOnPickup)
                pl.Items.Add(Data);
            LoadData();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<Player>())
        {
            IsInReach = true;
        }
        TryPickUp();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<Player>())
        {
            IsInReach = false;
        }
    }
}

[CreateAssetMenu(fileName = "Item", menuName = "ItemAsset", order = 1)]
public class Item : ScriptableObject
{
    public string Name;
    public string OnUseMethodName;
    public Sprite InventoryIcon;
    public Sprite WorldIcon;
    public Vector2 WorldSize;
    public Vector2 UISize;
    public Vector2 TriggerSize;
    public bool CollectItemOnPickup;
}