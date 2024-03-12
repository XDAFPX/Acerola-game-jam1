using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuskBloodFactory : Enemy
{
    public bool IsBossFightStarted;
    public LookAt Head;
    public int InAttack = 10;
    public bool CanAttack = true;
    public Bullet MinigunBullet;
    public Transform[] Hands;
    public Transform[] Miniguns;
    private int handindex =0;
    public GameObject Wall;
    public AudioClip GunShot;
    public void StartBossFight()
    {
        IsImmortal = false;
        IsBossFightStarted = true;
        Wall.SetActive(true);
        Invoke(nameof(ResetAttack), 2);
    }
    public void FixedUpdate()
    {
        var sizex = 0.5f;
        if (TargetPlayer.transform.position.x < transform.position.x) { sizex = 1.5f; Head.faceRight = true; }
        if (TargetPlayer.transform.position.x > transform.position.x) {sizex = -1.5f; Head.faceRight = false; }
        transform.localScale = new Vector2(sizex, 1.5f);
        if (IsBossFightStarted&&!IsDead)
        {
            if (InAttack == 0)
            {
                RollAttacks();
                //Debug.Log(InAttack.ToString());
            }
        }
    }
    public void Dark()
    {
        PostProcesingManager.Singleton.IsTriggered = true;
    }
    public override void Die()
    {
        GetComponent<Animator>().SetBool("IsDead", true);
        IsDead = true;
        TargetPlayer.IsImmortal = true;
        Head.enabled = false;

        //base.Die();
    }
    public void WIN()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
    }
    public void RollAttacks()
    {

        var rand =  Random.Range(1, 3);
        if (rand == 1)
        {
            StartCoroutine(StartAttack1());
        }
        if (rand == 2)
        {
            StartCoroutine(StartAttack2());
        }
    }


    public IEnumerator StartAttack1()
    {
        InAttack = 1;
        HandDown();
        yield return new WaitForSeconds(1);
        HandDown();
        yield return new WaitForSeconds(1.5f);
        HandDown();
        yield return new WaitForSeconds(1.5f);
        HandDown();
        yield return new WaitForSeconds(1.5f);
        HandDown();
        InAttack = 0;
    }
    public IEnumerator StartAttack2()
    {
        InAttack = 2;
        Miniguns[0].GetChild(0).GetComponent<Animator>().Play("MinigunMain");
        Miniguns[1].GetChild(0).GetComponent<Animator>().Play("MinigunMain");
        yield return new WaitForSeconds(0.4f);
        Transform Minigunpoint1 = Miniguns[0].GetChild(0).GetChild(0).GetChild(0);
        Transform Minigunpoint2 = Miniguns[1].GetChild(0).GetChild(0).GetChild(0);
        for (int i = 0; i < 6; i++)
        {
            GetComponent<AudioSource>().PlayOneShot(GunShot);
            var b1=Instantiate(MinigunBullet, Minigunpoint1.position, Minigunpoint1.rotation);
            var b2 = Instantiate(MinigunBullet, Minigunpoint2.position,Minigunpoint2.rotation);
            b1.GetComponent<Rigidbody2D>().AddForce(b1.transform.right * -30, ForceMode2D.Impulse);
            b2.GetComponent<Rigidbody2D>().AddForce(b2.transform.right * 30, ForceMode2D.Impulse);
            b1.transform.localScale = new Vector2(2, 2);
            b2.transform.localScale = new Vector2(-2, 2);
            b1.owner = this;
            b2.owner = this;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(3f);
        InAttack = 0;
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
