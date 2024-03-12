using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cultist : Enemy
{
    public float Damage;
    public float Ckonckback;
    public bool CanHit = true;
    public Transform[] StepingRaySpots;
    public Transform Grapchics;
    public GameObject CultistDead;
    public LayerMask Ground;
    private void FixedUpdate()
    {
        var dir = Vector2.zero;

        if (TargetPlayer.transform.position.x < transform.position.x && DetectedPlayer) { dir = Vector2.left; }
        if (TargetPlayer.transform.position.x > transform.position.x && DetectedPlayer) {dir = Vector2.right; }
        
        TryStep(dir);
    }
    private void LateUpdate()
    {
        if (TargetPlayer.transform.position.x < transform.position.x && DetectedPlayer) { Grapchics.localScale = new Vector3(0.5f, Grapchics.localScale.y, 1f); }
        if (TargetPlayer.transform.position.x > transform.position.x && DetectedPlayer) { Grapchics.localScale = new Vector3(-0.5f, Grapchics.localScale.y, 1f); }
    }
    private void TryStep(Vector2 flatwasd)
    {
        bool canmove = true;
        if (flatwasd.x == 0) return;
        Vector2 lowstepPos;
        Vector2 highstepPos;
        if (flatwasd.x == 1)
        {
            lowstepPos = StepingRaySpots[0].position;
            highstepPos = StepingRaySpots[1].position;
        }
        else
        {
            lowstepPos = StepingRaySpots[2].position;
            highstepPos = StepingRaySpots[3].position;
        }

        if (Physics2D.Raycast(lowstepPos, flatwasd, 0.1f,Ground))
        {
            if (!Physics2D.Raycast(highstepPos, flatwasd, 0.2f,Ground))
            {
                rb.position += new Vector2(0, 1.25f);
                rb.velocity += flatwasd * 1f;
            }
            else
            {
                canmove = false;
            }
        }
        if (DetectedPlayer&&canmove)
            RBPathFind(TargetPlayer.transform.position);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Player pl) && CanHit)
        {
            GetComponent<AudioSource>().Play();
            CanHit = false;
            Invoke(nameof(ResetHit), 2);
            GetComponent<Animator>().Play("CultistAttack");
            pl.TakeDamage(Damage);
            pl.rb.AddForce((pl.transform.position - transform.position) * Ckonckback, ForceMode2D.Impulse);
            pl.rb.AddForce(Vector2.up * Ckonckback / 2, ForceMode2D.Impulse);
        }
    }

    public void ResetHit()
    {
        CanHit = true;
    }
    public override void OnDie()
    {

        Instantiate(CultistDead, Physics2D.Raycast(transform.position, Vector2.down, 1000000f, Ground).point, Quaternion.Euler(0,0,-90));
        base.OnDie();
    }

}
