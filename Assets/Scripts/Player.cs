using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Damageble
{
    public InputActionAsset actions;
    public float Speed;
    public float JumpPower;
    public LayerMask Ground;
    public float Drag;
    public Ammo PistolAmmo;
    public Ammo ShotgunAmmo;
    public Transform GunSpot;
    public bool HasPistol;
    public bool HasShotgun;
    public bool Grounded { get { if (Physics2D.Raycast(transform.position, Vector2.down, 2.1f,Ground)) { return true; } return false; } }
    private Rigidbody2D rb;
    private InputAction moveAction;
    private bool canJump = true;
    private bool canRoll = true;
    public bool IsInRoll;
    void Awake()
    {
        actions.FindActionMap("MoveMent").FindAction("Jump").performed += OnJump;
        actions.FindActionMap("MoveMent").FindAction("Roll").performed += OnRoll;
        moveAction = actions.FindActionMap("MoveMent").FindAction("WASD");
    }
    public void ResetJump() { canJump = true; }
    public void ResetRoll() { canRoll = true; }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    public void FixedUpdate()
    {
        OnWalk(moveAction.ReadValue<Vector2>());
    }
    public static float InQuint(float t) => t * t * t * t * t;
    public static float InOutQuint(float t)
    {
        if (t < 0.5) return InQuint(t * 2) / 2;
        return 1 - InQuint((1 - t) * 2) / 2;
    } //coolest one
    public void OnRoll(InputAction.CallbackContext context)
    {
        if (canRoll && Grounded)
        {
            Vector2 flatwasd = new Vector2(moveAction.ReadValue<Vector2>().x, 0);
            if (flatwasd == Vector2.zero)
                flatwasd = Vector2.right;
            Vector2 telepos = new Vector2(transform.position.x,transform.position.y) + flatwasd * 3;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, flatwasd, 3, Ground);
            if (hit)
            {
                telepos = hit.point;
            }
            StartCoroutine(DoRoll(2, transform.position, telepos));

            IsInRoll = true;
            canRoll = false;
            Invoke(nameof(ResetRoll), 1);
            
        }
    }
    public IEnumerator DoRoll(float speed,Vector2 initpos,Vector2 telepos)
    {
        float time = 0;
        while (time < 1)
        {
            float t = InOutQuint(time);
            transform.position = Vector2.Lerp(initpos, telepos, t);

            time += Time.deltaTime * speed;
            yield return null;
        }
        IsInRoll = false;
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (Grounded && canJump&&!IsInRoll)
        {
            canJump = false;
            Invoke(nameof(ResetJump), 0.8f);
            rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
        }

    }

    public void OnWalk(Vector2 wasd)
    {
        if (IsInRoll) return;
        Vector2 flatwasd = new Vector2(wasd.x, 0);
        if (Grounded)
        {
            rb.drag = Drag;
            rb.AddForce(flatwasd * Speed);
        }
        else
        {
            rb.drag = 0;
            rb.AddForce(flatwasd * Speed*0.6f);
        }
        if(flatwasd.x == 0&& Grounded)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

    }
    public int GetNeededAmmoCount(BaseGun.AmmoType type)
    {
        if(type == BaseGun.AmmoType.bullet)
        {
            return PistolAmmo.count;
        }
        else
        {
            return ShotgunAmmo.count;
        }
    }
    public void WriteNeededAmmoCount(BaseGun.AmmoType type,int count)
    {
        if (type == BaseGun.AmmoType.bullet)
        {
            PistolAmmo = new Ammo(count, PistolAmmo.type);
        }
        else
        {
            ShotgunAmmo = new Ammo(count, ShotgunAmmo.type);
        }
    }
    void OnEnable()
    {
        actions.FindActionMap("MoveMent").Enable();
    }
    void OnDisable()
    {
        actions.FindActionMap("MoveMent").Disable();
    }

    public override void Die()
    {
        
    }
}
