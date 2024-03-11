using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damageble : MonoBehaviour
{
    public float MaxHp;
    public float Hp;
    public bool IsImmortal;
    public bool IsDead;
    public virtual void TakeDamage(float value)
    {
        if(!IsImmortal)
           Hp =  Mathf.Clamp( Hp - value,0,1000);
        if (Hp == 0)
        {
            IsDead = true;
            Die();
        }

    }
    public virtual void HealDamage(float value)
    {
        if (!IsImmortal)
            Hp = Mathf.Clamp(Hp + value, 0, MaxHp);
        if (Hp == 0)
        {
            IsDead = true;  
            Die();
        }

    }
    public abstract void Die();
}
