using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : BaseGun
{
    public Transform Bullet;
    private void OnEnable()
    {
        owner.grapchics.transform.Find("arm2").localRotation = Quaternion.Euler(0, 0, 76);
        owner.grapchics.transform.Find("arm2").localPosition= new Vector3(-0.06f, -0.39f,0);
        owner.grapchics.transform.Find("arm2").GetComponent<SpriteRenderer>().sortingOrder = 3;
    }
    private void OnDisable()
    {
        owner.grapchics.transform.Find("arm2").localRotation = Quaternion.Euler(0, 0, 0);
        owner.grapchics.transform.Find("arm2").localPosition = new Vector3(-0.5f, -0.458f, 0);
        owner.grapchics.transform.Find("arm2").GetComponent<SpriteRenderer>().sortingOrder = -3;
    }
    public override void Fire()
    {
        CameraShaker.Singleton.StartShake(4, 0.5f, 0.1f);
        GetComponent<Animator>().Play("RevolverFire",0,0);
        var hit = Physics2D.Raycast(FireSpot.position, transform.right, 100000f,WhatToHit);
        var bullet = Instantiate(Bullet, FireSpot.position, Quaternion.identity);
        bullet.transform.right = transform.right;
        Vector2 endpos= Vector2.zero;
        if (hit)
        {

            if (FindLastPerent(hit.transform).TryGetComponent(out Damageble damageble))
            {
                damageble.TakeDamage(Damage);
            }

            endpos = hit.point;
        }
        else
            endpos = FireSpot.position + transform.right * 40;
        StartCoroutine(ShootBullet(7, FireSpot.position, endpos, bullet));
    }

    public static  Transform FindLastPerent(Transform child)
    {
        bool foundperrent = true;
        Transform nchild = child;
        while (foundperrent)
        {
            Transform tchild = nchild;
            nchild = nchild.parent;
            if(nchild == null)
            {
                foundperrent = true;
                return tchild;
            }
        }
        return child;
    }

    private void FixedUpdate()
    {
        IsActive = owner.HasPistol;
    }
    public IEnumerator ShootBullet(float speed,Vector2 initpos,Vector2 endpos,Transform tr)
    {
        float time = 0;
        while (time < 1)
        {
            float t = Player.InOutQuint(time);
            tr.position = Vector2.Lerp(initpos, endpos, t);

            time += Time.deltaTime * speed;
            yield return null;
        }
        Destroy(tr.gameObject);
    }
    public override void TriggerOnReload()
    {
        GetComponent<Animator>().Play("RevolverReload");
        owner.CanSwitchWeapons = false;
        base.TriggerOnReload();
    }
    public override void TriggerAfterReload()
    {
        owner.CanSwitchWeapons = true;
        base.TriggerAfterReload();
    }
}
