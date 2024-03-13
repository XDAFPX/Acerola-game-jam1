using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Credits : MonoBehaviour
{
    private TextMeshPro t1;
    private TextMeshPro t2;
    private TextMeshPro t3;
    private void Start()
    {
        t1 = transform.Find("2").GetComponent<TextMeshPro>();
        t2 = transform.Find("3").GetComponent<TextMeshPro>();
        t3 = transform.Find("1").GetComponent<TextMeshPro>();
        StartCoroutine(Creditz());
    }
    public IEnumerator Creditz()
    {
        StartCoroutine(PrintString(0.2f,"Bloodshed has been stoped.Humanity is trully doomed",1));
        yield return new WaitForSeconds(12);
        StartCoroutine(PrintString(0.1f, "YOU DID IT.YOU SURVIVED IN WASTELAND", 2));
        yield return new WaitForSeconds(7);
        StartCoroutine(PrintString(0.2f, "Thank you for playing.", 3));
        yield return new WaitForSeconds(4);
        Application.Quit();
    }
    public void Clear()
    {
        t1.text = "";
        t2.text = "";
        t3.text = "";
    }
    public IEnumerator PrintString(float Speed,string s,int t)
    {
        Clear();
        TextMeshPro text = null;
        if (t == 1) text = t1;
        if (t == 2) text = t2;
        if (t == 3) text = t3;
        for (int i = 0; i < s.Length; i++)
        {
            yield return new WaitForSeconds(Speed);
            text.text += s[i];
        }


    }
}
