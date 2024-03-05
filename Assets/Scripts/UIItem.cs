using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{
    public Item Data;
    public int Index;
    private void Start()
    {
        LoadData();
    }
    public void LoadData()
    {
        if (Data)
        {
            GetComponent<Image>().sprite = Data.InventoryIcon;
            transform.localScale = Data.UISize;
        }
    }
    public void CLick()
    {
        if(Data.OnUseMethodName != "")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Invoke(Data.OnUseMethodName, 0);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Items.RemoveAt(Index);
            Destroy(gameObject);
        }
        
    }
}
