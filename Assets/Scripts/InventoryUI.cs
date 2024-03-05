using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : UIElement
{
    public Player pl;
    public UIItem GenericItem;
    public Transform scrollbar;
    private void Start()
    {
        pl = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public override void OpenElement()
    {
        float margin = 1.2f;
        for (int i = 0; i < pl.Items.Count; i++)
        {
            var item = Instantiate(GenericItem, new Vector3(scrollbar.parent.position.x + margin, scrollbar.parent.position.y-0.4f), Quaternion.identity);

            item.transform.SetParent( scrollbar);
            item.Data = pl.Items[i];
            item.Index = i;
            margin += 1;
        }

        base.OpenElement();
    }
    
    public override void CloseElement()
    {
        for (int i = 0; i < scrollbar.childCount; i++)
        {
            Destroy(scrollbar.GetChild(i).gameObject);
        }
        base.CloseElement();
    }
}
