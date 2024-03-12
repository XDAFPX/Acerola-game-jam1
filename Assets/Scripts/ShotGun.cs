using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : BaseGun
{
    public Transform Bullet;
    public float ScaterValue;
    public int Shots;
    private void OnEnable()
    {
        owner.grapchics.transform.Find("arm1").localPosition = new Vector3(0.73f, -0.287f, 0);
        owner.grapchics.transform.Find("arm1").localRotation = Quaternion.Euler(0, 0, 34.902f);
        owner.grapchics.transform.Find("arm2").localRotation = Quaternion.Euler(0, 0, 44.16f);
        owner.grapchics.transform.Find("arm2").localPosition = new Vector3(-0.31f, -0.55f, 0);
        owner.grapchics.transform.Find("arm2").GetComponent<SpriteRenderer>().sortingOrder = 3;
        owner.grapchics.transform.Find("arm1").GetComponent<SpriteRenderer>().sortingOrder = -3;
    }

    private void OnDisable()
    {
        owner.grapchics.transform.Find("arm1").localPosition =new  Vector3(0.48f, -0.44f, 0);
        owner.grapchics.transform.Find("arm1").localRotation = Quaternion.Euler(0, 0, 0);
        owner.grapchics.transform.Find("arm2").localRotation = Quaternion.Euler(0, 0, 0);
        owner.grapchics.transform.Find("arm2").localPosition = new Vector3(-0.5f, -0.458f, 0);
        owner.grapchics.transform.Find("arm2").GetComponent<SpriteRenderer>().sortingOrder = -3;
        owner.grapchics.transform.Find("arm1").GetComponent<SpriteRenderer>().sortingOrder = -3;
    }
    public override void Fire()
    {
        CameraShaker.Singleton.StartShake(7, 0.5f, 0.1f);
        GetComponent<Animator>().Play("ShotgunIdle", 0, 0);
        for (int i = 0; i < Shots; i++)
        {
            Vector3 right = new Vector2(transform.right.x + Random.Range(-ScaterValue, ScaterValue), transform.right.y + Random.Range(-ScaterValue, ScaterValue));
            var hit = Physics2D.Raycast(FireSpot.position,right , 100000f, WhatToHit);
            var bullet = Instantiate(Bullet, FireSpot.position, Quaternion.identity);
            bullet.transform.right = transform.right;
            Vector2 endpos = Vector2.zero;
            if (hit)
            {

                if (Pistol.FindLastPerent(hit.transform).TryGetComponent(out Damageble damageble))
                {
                    damageble.TakeDamage(Damage);
                }
                endpos = hit.point;
            }
            else
                endpos = FireSpot.position + right * 20;
            StartCoroutine(ShootBullet(4, FireSpot.position, endpos, bullet));
        }
    }
    private void FixedUpdate()
    {
        IsActive = owner.HasShotgun;
    }
    public IEnumerator ShootBullet(float speed, Vector2 initpos, Vector2 endpos, Transform tr)
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
        GetComponent<Animator>().Play("ShotgunReload");
        owner.CanSwitchWeapons = false;
        base.TriggerOnReload();
    }
    public override void TriggerAfterReload()
    {
        owner.CanSwitchWeapons = true;
        base.TriggerAfterReload();
    }
}
