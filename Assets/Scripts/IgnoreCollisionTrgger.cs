using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisionTrgger : MonoBehaviour
{
    public Collider2D IgnoreObject; 
    private void Awake()
    {
        if(IgnoreObject)
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), IgnoreObject);
    }
}
