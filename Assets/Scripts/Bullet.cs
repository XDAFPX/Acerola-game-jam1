using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Damageble owner;
    public float Damage;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Damageble damageble))
        {
            if(!ReferenceEquals( damageble.gameObject, owner.gameObject))
                damageble.TakeDamage(Damage);
        }
        Destroy(gameObject);
    }
}
