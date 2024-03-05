using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public abstract class Enemy : Damageble
{
    public Player TargetPlayer;
    public float Range;
    public float Speed;
    protected bool DetectedPlayer { get { return (DistanceToPlayer > Range); } }

    protected AIDestinationSetter ai;
    protected Rigidbody2D rb;
    public float DistanceToPlayer { get { return Vector2.Distance(transform.position, TargetPlayer.transform.position); } }

    private void Start()
    {
        ai = GetComponent<AIDestinationSetter>();
        rb = GetComponent<Rigidbody2D>();
    }
    public override void Die()
    {
        
    }

    public void StartPathFind(Transform target)
    {
        if(ai)
            ai.target = target;
        RBPathFind(target);
    }
    public void StopPahfind()
    {
        if(ai)
            ai.target = null;
        RBPathFind(null);
    }
    public void RBPathFind(Transform tr)
    {
        if (rb&&tr)
        {
            Vector2 targ = (tr.position - transform.position).normalized;
            rb.velocity = targ * Speed;
        }
        if (tr)
            rb.velocity = Vector2.zero;
    }
}
