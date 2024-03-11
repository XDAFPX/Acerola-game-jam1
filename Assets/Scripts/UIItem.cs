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
            transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = Data.Name;
        }
    }
    public void CLick()
    {
        if(Data.OnUseMethodName != "")
        {
            var pl = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            if (pl.Items.Count>Index)
            {
                pl.Invoke(Data.OnUseMethodName, 0);
                pl.Items.RemoveAt(Index);
            }
            Destroy(this.gameObject);
        }
        
    }
}
