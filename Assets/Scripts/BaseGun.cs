using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseGun : MonoBehaviour
{
    public float Damage;
    public bool canShoot;
    public float ReloadTime;
    public float TimeBeetwenBullets;
    public int MaxAmmo;
    public int CurrentAmmo;
    public AmmoType _AmmoType;
    public Transform FireSpot;
    public Player owner;
    public LayerMask WhatToHit;
    public InputActionAsset Actions;
    public bool IsActive = true;
    private InputAction fireinput; public AudioClip FireSound;
    public float preferebledir =1 ;
    public enum AmmoType
    {
        _default,
        bullet,
        shotgunshels
    }
    private void Update()
    {
        if(owner && IsActive )
            LookAtCursor();
        if(fireinput.ReadValue<float>() == 1)
        {
            TryFire();
        }
    }
    private void Start()
    {
        fireinput = Actions.FindActionMap("GunPlay").FindAction("Fire");
        AmmoUI.Singleton.UpdateAmmo(CurrentAmmo, MaxAmmo, owner.GetNeededAmmoCount(_AmmoType));
    }




    public virtual void LookAtCursor()
    {
        Vector3 mousePositionScreen = Input.mousePosition;
        // Convert the mouse position to world space
        Vector3 mousePositionWorld = Camera.main.ScreenToWorldPoint(mousePositionScreen);
        var dir = mousePositionWorld - transform.position;
        float angleRadians = Mathf.Atan2(dir.y, dir.x);
        // Convert the angle from radians to degrees and apply it to the Z rotation
        float angleDegrees = Mathf.Rad2Deg * angleRadians;
        transform.parent.rotation = Quaternion.Euler(0f, 0f, angleDegrees);
        if (angleDegrees <= 90 && angleDegrees >= -90) preferebledir = 1;
        else preferebledir = -1;
    }

    public void TryFire()
    {
        if (!owner && !IsActive) return;
        if (owner.IsDead||Player.ShowCursor||owner.IsInDialoge) return;
        if(CurrentAmmo >= 1)
        {
            if (canShoot)
            {
                Mathf.Clamp(CurrentAmmo--, 0, 1000);
                AmmoUI.Singleton.UpdateAmmo(CurrentAmmo, MaxAmmo, owner.GetNeededAmmoCount(_AmmoType));
                GetComponent<AudioSource>().PlayOneShot(FireSound);
                Fire();
                StartCoroutine(TimeBetweenShots());
            }
           
        }
        else
        {
            if (owner.GetNeededAmmoCount(_AmmoType) >= MaxAmmo && canShoot)
            {
                StartCoroutine(Reload(MaxAmmo));
            }
            else if(owner.GetNeededAmmoCount(_AmmoType) ==0)
                TriggerNoAmmo();
            else if (canShoot)
            {
                StartCoroutine(Reload(owner.GetNeededAmmoCount(_AmmoType)));
            }
        }
    }
    public abstract void Fire();

    public IEnumerator TimeBetweenShots()
    {
        canShoot = false;
        yield return new WaitForSeconds(TimeBeetwenBullets);
        canShoot = true;
    }
    public IEnumerator Reload(int ammoamount)
    {
        AmmoUI.Singleton.UpdateAmmo(CurrentAmmo, MaxAmmo, owner.GetNeededAmmoCount(_AmmoType));
        canShoot = false;
        TriggerOnReload();
        yield return new WaitForSeconds(ReloadTime);
        
        owner.WriteNeededAmmoCount(_AmmoType, owner.GetNeededAmmoCount(_AmmoType) - ammoamount);
        CurrentAmmo = ammoamount;
        AmmoUI.Singleton.UpdateAmmo(CurrentAmmo, MaxAmmo, owner.GetNeededAmmoCount(_AmmoType));
        TriggerAfterReload();
        canShoot = true;
    }
    public virtual void TriggerOnReload()
    {

    }
    public virtual void TriggerAfterReload()
    {

    }
    public virtual void TriggerNoAmmo()
    {

    }
    void OnEnable()
    {
        Actions.FindActionMap("GunPlay").Enable();
    }

}
[System.Serializable]
public struct Ammo
{
    public int count;
    public BaseGun.AmmoType type;
    public Ammo(int _count,BaseGun.AmmoType _type)
    {
        count = _count;
        type = _type;
    }
    public static Ammo operator + (Ammo a,Ammo b)
    {
        if (a.type == b.type)
            return new Ammo(a.count + b.count,a.type);
        
        return new Ammo();
    }
    public static Ammo operator -(Ammo a, Ammo b)
    {
        if (a.type == b.type)
            return new Ammo(a.count - b.count, a.type);

        return new Ammo();
    }
    public static Ammo operator -(Ammo a, int b)
    {
        return new Ammo(a.count - b, a.type);
    }
    public static Ammo operator +(Ammo a, int b)
    {
        return new Ammo(a.count + b, a.type);
    }
}
