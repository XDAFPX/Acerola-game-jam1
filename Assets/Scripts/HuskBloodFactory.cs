using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuskBloodFactory : Enemy
{
    public bool IsBossFightStarted;
    public LookAt Head;
    public int InAttack = 10;
    public bool CanAttack = true;
    public Transform[] Hands;
    private int handindex =0;
    public void StartBossFight()
    {
        IsBossFightStarted = true;
    }
    public void Update()
    {
        var sizex = 0.5f;
        if (TargetPlayer.transform.position.x < transform.position.x) { sizex = 1.5f; Head.faceRight = true; }
        if (TargetPlayer.transform.position.x > transform.position.x) {sizex = -1.5f; Head.faceRight = false; }
        transform.localScale = new Vector2(sizex, 1.5f);
        if (IsBossFightStarted)
        {
            if (InAttack == 10)
                Invoke("ResetAttack", 3f);
            if (InAttack == 0)
            {
                RollAttacks();
                //Debug.Log(InAttack.ToString());
            }
        }
    }
    public void RollAttacks()
    {

        var rand = 1; //Random.Range(1, 4);
        if (rand == 1)
        {
            StartAttack1();
        }
    }


    public void StartAttack1()
    {
        InAttack = 1;
        Invoke(nameof(HandDown), 1);
        Invoke(nameof(HandDown), 2.5f);
        Invoke(nameof(HandDown), 4.5f);
        Invoke(nameof(HandDown), 7f);
        Invoke(nameof(HandDown), 9f);
        Invoke(nameof(ResetAttack), 12f);
    }
    public override void OnDie()
    {
        
    }
    public void HandDown()
    {

        if (handindex == 1) handindex--;
        else if (handindex == 0) handindex++;
        Hands[handindex].GetChild(0).GetComponent<Animator>().Play("HuskHandDown");
        Hands[handindex].transform.position = TargetPlayer.transform.position;
    }
    public void ResetAttack()
    {
        InAttack = 0;
    }
}
