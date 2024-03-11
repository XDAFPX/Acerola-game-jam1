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
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Data.WorldIcon;
                transform.localScale = Data.WorldSize;
                GetComponent<BoxCollider2D>().size = Data.TriggerSize;
            }
            else
            {
                //transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Data.InventoryIcon;
                Destroy(gameObject);
            }
        }
    }
    public void TryPickUp()
    {
        if (IsInReach && !IsInInventory)
        {
            Player pl = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            if (PickupParticle&&UnityEngine.SceneManagement.SceneManager.GetActiveScene().name=="P1_a1")
                Instantiate(PickupParticle, transform.position, Quaternion.identity);
            IsInInventory = true;
            if (Data.CollectItemOnPickup)
                pl.Items.Add(Data);
            if (Data.OnPickupMethodName != "")
                pl.Invoke(Data.OnPickupMethodName, 0);
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

