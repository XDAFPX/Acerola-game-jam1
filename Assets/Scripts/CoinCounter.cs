using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    public static CoinCounter Singleton;

    private void Awake()
    {
        Singleton = this;
    }
    public void LoadCoinData()
    {
        GetComponent<TextMeshProUGUI>().text = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Coins.ToString()+"$";
    }
}
