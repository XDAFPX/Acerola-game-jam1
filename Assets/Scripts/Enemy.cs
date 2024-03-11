using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Enemy : Damageble
{
    public Player TargetPlayer;
    public float Range;
    public float Speed;
    protected bool DetectedPlayer { get { return (DistanceToPlayer < Range); } }

    protected Rigidbody2D rb;
    public float DistanceToPlayer { get { return Vector2.Distance(transform.position, TargetPlayer.transform.position); } }

    private void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        TargetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    public override void Die()
    {
        OnDie();
        Destroy(gameObject);
    }
    public virtual void  OnDie()
    {

    }
    public void PathFind(Transform target)
    {

        RBPathFind(target.position);
    }
    public void StopPahfind()
    {

        RBPathFind(Vector3.zero);
    }
    public void RBPathFind(Vector3 pos)
    {
        if (rb)
        {
            Vector2 targ = (pos - transform.position).normalized; 
            rb.velocity = new Vector2((targ* Speed).x, rb.velocity.y);
        }
        if (pos == Vector3.zero)
            rb.velocity = Vector3.zero;
    }
    public void RBPathFind(Vector2 dir)
    {
        if (rb)
        {
            rb.velocity = new Vector2((dir * Speed).x, rb.velocity.y);
        }
        if (dir == Vector2.zero)
            rb.velocity = Vector3.zero;
    }
}
