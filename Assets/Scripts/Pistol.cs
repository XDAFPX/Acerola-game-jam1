using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : BaseGun
{
    public override void Fire()
    {
        var hit = Physics2D.Raycast(FireSpot.position, transform.forward, 100000f,WhatToHit);
        if (hit)
        {
            
            if (hit.transform.TryGetComponent(out Damageble damageble))
            {
                damageble.TakeDamage(Damage);
            }
        }
    }
    private void FixedUpdate()
    {
        IsActive = owner.HasPistol;
    }
}
