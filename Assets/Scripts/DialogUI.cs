using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogUI : MonoBehaviour
{
    public static DialogUI Singleton;
    public TMPro.TextMeshProUGUI MainTMP;
    public TMPro.TextMeshProUGUI NameTMP;
    private void Awake()
    {
        Singleton = this;
    }
    private string[] Senteces;
    private string Name;
    private float Speed;
    public Dialogtrigger IsinDialog = null;
    public int SenteceIndex;
    

    public void LoadData(string[] senteces,string name,float speed)
    {
        Senteces = senteces; Name = name; Speed = speed;
    }
    public void DisposeData()
    {
        Senteces = default;
        SenteceIndex = 0;
        Name = "";
        Speed = default;
        MainTMP.text = "";
        NameTMP.text = "";
    }
    public void Interact(Dialogtrigger brodcaster)
    {
        if (IsinDialog)
        {
            StopAllCoroutines();
            if (SenteceIndex == Senteces.Length)
            {
                IsinDialog = null; SetPlayerInDialog(); DisposeData();
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Camera.Follow = GameObject.FindGameObjectWithTag("Player").transform; return;
            }



            StartCoroutine(PrintString(Senteces[SenteceIndex]));
            Mathf.Clamp(SenteceIndex++, 0, Senteces.Length);

        }
        else
        {
            LoadData(brodcaster.Senteces, brodcaster.Name, brodcaster.TimePerCharacter);
            SenteceIndex = 0;
            StartCoroutine(PrintString(Senteces[SenteceIndex]));
            Mathf.Clamp(SenteceIndex++, 0, Senteces.Length);
            IsinDialog = brodcaster;
            SetPlayerInDialog();
            NameTMP.text = Name;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Camera.Follow = brodcaster.transform;
        }
    }
    public void StopDialog()
    {
        IsinDialog = null; SetPlayerInDialog(); DisposeData();
        StopAllCoroutines();
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Camera.Follow = GameObject.FindGameObjectWithTag("Player").transform; return;
    }
    public IEnumerator PrintString(string s)
    {
        MainTMP.text = "";
        for (int i = 0; i < s.Length; i++)
        {
            yield return new WaitForSeconds(Speed);
            MainTMP.text += s[i];
        }
    }
    public void SetPlayerInDialog()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().IsInDialoge = IsinDialog;
    }

}
