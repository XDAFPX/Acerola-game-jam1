using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Diagnostics;
public class InventoryUI : UIElement
{
    public Player pl;
    public UIItem GenericItem;
    public Transform scrollbar;
    public SpecialEffect Glitch;
    public Dialogtrigger SpecialDialog;
    public GameObject referencetospecial;
    private void Start()
    {
        pl = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public override void OpenElement()
    {
        float margin = 1.6f;
        bool specialDetected =false;

        for (int i = 0; i < pl.Items.Count; i++)
        {
            if (specialDetected)
            {
                for (int j = 0; j < 5; j++)
                {
                    var g = Instantiate(Glitch, new Vector3(scrollbar.parent.position.x + margin, scrollbar.parent.position.y - 1f), Quaternion.identity); g.transform.SetParent(scrollbar); margin += 1;
                }
                SpecialEvent();
                specialDetected = false;
                break;
            }
            else
            {
                var item = Instantiate(GenericItem, new Vector3(scrollbar.parent.position.x + margin, scrollbar.parent.position.y - 1f), Quaternion.identity);

                item.transform.SetParent(scrollbar);
                item.Data = pl.Items[i];
                
                item.Index = i;
                if (pl.Items[i].ISSPECIAL) { specialDetected = true; referencetospecial = item.gameObject; }
                if (i + 1 < pl.Items.Count) { if (pl.Items[i + 1].ISSPECIAL) margin += 2; else margin += 1; }
                else margin += 1;
            }
            
        }
        if(specialDetected&&referencetospecial != null)
        {
            SpecialEvent();
        }
        base.OpenElement();
    }
    public void SpecialEvent()
    {
        scrollbar.parent.GetComponent<Mask>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().IsInDialoge = true;
        Invoke(nameof(SpecialEventpart2), 1);
    }
    public void SpecialEventpart2()
    {
        var item = referencetospecial;
        item.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform);
        item.transform.GetChild(0).gameObject.SetActive(false);
        DialogUI.Singleton.transform.SetParent(null);
        DialogUI.Singleton.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform);
        DialogUI.Singleton.transform.localPosition = Vector3.zero;
        DialogUI.Singleton.transform.localScale = new Vector3(2,2);
        DialogUI.Singleton.Interact(SpecialDialog);

        item.GetComponent<RectTransform>().position = Vector3.zero;
        item.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        item.GetComponent<RectTransform>().localPosition = Vector3.zero;
        item.transform.localScale = new Vector3(15, 15);
        PostProcesingManager.Singleton.IsTriggered = true;
        CameraShaker.Singleton.StartShake(5, 4);
        if(SaveNLoadManager.Singleton)
            SaveNLoadManager.Singleton.Save(new SaveData(new Item[0], 100, 2, 32, 20, false));
        StartCoroutine(PrintString("PRes 'E' to ki11 suRvi7or - fexjicute iof opp66 mpdafpgpgmb0-2r,2[42(fksfs0f(Wif'ds0i_F(fdsfopks ierp ww opsko ks0r-w0e-0-ifkSh-k0-0 w0qi0vsfs0fgis-0fis9&0_&*_ gs-fs8f sfsfksfsffspojjwefwsekfsgksdggp[tgsoirkigssoijgsij[aas94tvjafgsa[ktg(*fdsf90*GFudvgS_G9U*D0g-S9sedg80*SDg8SD0gf9G(*GF9d_"));
        Invoke(nameof(SpecialEventpart4),4.5f);
    }
    public void SpecialEventpart4()
    {
        Debug.LogWarning("CrashHere!");
#if UNITY_EDITOR
        return;
#endif
#pragma warning disable CS0162 // Unreachable code detected
        Utils.ForceCrash(ForcedCrashCategory.FatalError);
#pragma warning restore CS0162 // Unreachable code detected

    }
    public IEnumerator PrintString(string s)
    {
        string text = "";
        for (int i = 0; i < s.Length; i++)
        {
            yield return 0;
            text += s[i];
            HintUI.Singleton.ShowHint(text);
        }


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
