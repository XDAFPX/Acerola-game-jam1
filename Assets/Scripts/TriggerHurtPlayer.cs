using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHurtPlayer : MonoBehaviour
{
    public float Damage;
    public float Cknockback;
    public void Hurt()
    {
        Player pl = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        pl.TakeDamage(Damage);
        pl.rb.AddForce((pl.transform.position - transform.position) * Cknockback, ForceMode2D.Impulse);
        pl.rb.AddForce(Vector2.up * Cknockback / 2, ForceMode2D.Impulse);
    }
}
