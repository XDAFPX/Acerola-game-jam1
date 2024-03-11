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

    private void FixedUpdate()
    {
        var dir = Vector2.zero;

        if (TargetPlayer.transform.position.x < transform.position.x&&DetectedPlayer) dir = Vector2.left;
        if (TargetPlayer.transform.position.x > transform.position.x&&DetectedPlayer) dir = Vector2.right;
        TryStep(dir);
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

        if (Physics2D.Raycast(lowstepPos, flatwasd, 0.1f))
        {
            if (!Physics2D.Raycast(highstepPos, flatwasd, 0.2f))
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
            CanHit = false;
            Invoke(nameof(ResetHit), 2);
            pl.TakeDamage(Damage);
            pl.rb.AddForce((pl.transform.position - transform.position) * Ckonckback, ForceMode2D.Impulse);
            pl.rb.AddForce(Vector2.up * Ckonckback / 2, ForceMode2D.Impulse);
        }
    }

    public void ResetHit()
    {
        CanHit = true;
    }

}
