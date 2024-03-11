using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    public static AmmoUI Singleton;
    private void Awake()
    {
        Singleton = this;
    }
    public void UpdateAmmo(int current,int max,int playersammo)
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = current.ToString() + "/" + max.ToString();
        transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = playersammo.ToString();
    }
    public void Erase()
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = "";
    }
    public void Show()
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = 0.ToString() + "/" + 0.ToString();
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void Hide()
    {
        Erase();
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
