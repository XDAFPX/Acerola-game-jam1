using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPitMonster : Enemy
{
    public ParticleSystem BallPitParticles;
    public Transform BallPitStart;
    public Transform BallPitEnd;
    public bool IsMoving;
    private bool CanSpawnBallPitParticles = true;
    public float Damage;
    public float Ckonckback;
    public bool CanHit = true;
    public bool CanMoveRight { get { return  transform.position.x < BallPitEnd.position.x; } }
    public bool CanMoveLeft { get { return BallPitStart.position.x < transform.position.x; } }
    public void FixedUpdate()
    {
        Vector2 dir = Vector2.zero;
        if (DetectedPlayer)
        {

            if (TargetPlayer.transform.position.x < transform.position.x&&CanMoveLeft) dir = Vector2.left;
            if (TargetPlayer.transform.position.x > transform.position.x&&CanMoveRight) dir = Vector2.right;
            if (CanSpawnBallPitParticles && (dir!=Vector2.zero))
            {

                Instantiate(BallPitParticles, transform.position, Quaternion.identity);
                CanSpawnBallPitParticles = false;
                Invoke(nameof(ResetParticleTimer), 0.5f);
            }
            //if (dir != Vector2.zero) CameraShaker.Singleton.StartShake(0.5f, 2); else CameraShaker.Singleton.StopShake();
        }
        RBPathFind(dir);
    }
    public void ResetParticleTimer()
    {
        CanSpawnBallPitParticles = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player pl) && CanHit)
        {
            CanHit = false;
            Invoke(nameof(ResetHit), 1);
            pl.TakeDamage(Damage);
            //pl.rb.AddForce((pl.transform.position - transform.position) * Ckonckback, ForceMode2D.Impulse);
            pl.rb.AddForce(Vector2.up * Ckonckback / 2, ForceMode2D.Impulse);
        }
    }
    public void  ResetHit()
    {
        CanHit = true;
    }
}
