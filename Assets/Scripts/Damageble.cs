using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damageble : MonoBehaviour
{
    public float Hp;
    public void TakeDamage(float value)
    {
        Mathf.Clamp( Hp -= value,0,1000);
        if (Hp == 0)
            Die();
    }
    public void HealDamage(float value)
    {
        Mathf.Clamp(Hp += value, 0, 1000);
        if (Hp == 0)
            Die();
    }
    public abstract void Die();
}
