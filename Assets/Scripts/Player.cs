using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

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
    public bool Grounded { get { if (Physics2D.Raycast(transform.position, Vector2.down, 1.15f,Ground)) { return true; } return false; } }
    private Rigidbody2D rb;
    private InputAction moveAction;
    private bool canJump = true;
    private bool canRoll = true;
    public bool IsInRoll;
    public BaseGun Magnum;
    public BaseGun Shotgun;
    public int weapon;
    public List<Item> Items = new List<Item>();
    private static bool _showcursor;
    public static bool ShowCursor { get { return Player._showcursor; } set { Player._showcursor = value; if (value) { Cursor.visible = value; Cursor.lockState = CursorLockMode.None; } else { Cursor.visible = false; Cursor.lockState = CursorLockMode.Confined; } } }
    private UIElement currentUIElement;
    public UIElement InventoryElement;
    public bool IsInIntro;
    public bool IsInDialoge;
    public int Coins;
    public CinemachineVirtualCamera Camera;
    void Awake()
    {
        actions.FindActionMap("MoveMent").FindAction("Jump").performed += OnJump;
        actions.FindActionMap("MoveMent").FindAction("Roll").performed += OnRoll;
        actions.FindActionMap("UI").FindAction("Close").performed += OnEsc;
        actions.FindActionMap("UI").FindAction("Inventory").performed += OnTab;
        moveAction = actions.FindActionMap("MoveMent").FindAction("WASD");
    }



    public void ResetJump() { canJump = true; }
    public void ResetRoll() { canRoll = true; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (IsInIntro)
        {
            StartCoroutine(DoRoll(0.5f, transform.position, transform.position + transform.right * 3,true));
        }
        CoinCounter.Singleton.LoadCoinData();
    }
    public void FixedUpdate()
    {
        OnWalk(moveAction.ReadValue<Vector2>());
        
        
    }
    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (weapon == 2)
            {
                weapon = 1;
                SwitchWeapon();
            }
            else
            {
                weapon++;
                SwitchWeapon();
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (weapon == 1)
            {
                weapon = 2;
                SwitchWeapon();
            }
            else
            {
                weapon--;
                SwitchWeapon();
            }
        }
        if (Input.GetKey(KeyCode.Alpha1))
        {
            weapon = 1;
            SwitchWeapon();
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            weapon = 2;
            SwitchWeapon();
        }
    }
    public void SwitchWeapon()
    {
        if(weapon == 1&&HasPistol)
        {
            Magnum.gameObject.SetActive(true);
            Shotgun.gameObject.SetActive(false);
        }
        if (weapon == 2 && HasShotgun)
        {
            Magnum.gameObject.SetActive(false);
            Shotgun.gameObject.SetActive(true);
        }
    }
    public static float InQuint(float t) => t * t * t * t * t;
    public static float InOutQuint(float t)
    {
        if (t < 0.5) return InQuint(t * 2) / 2;
        return 1 - InQuint((1 - t) * 2) / 2;
    } //coolest one
    public void OnTab(InputAction.CallbackContext obj)
    {
        if (IsInDialoge) return;
        var e =GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("Inventory").GetComponent<UIElement>();
        if(currentUIElement == e)
        {
            e.CloseElement();
            currentUIElement = null;
        }
        else if(currentUIElement == null)
        {
            currentUIElement = e;
            e.OpenElement();

        }
        
    }
    public void OnEsc(InputAction.CallbackContext context)
    {
        if (currentUIElement)
        {

            currentUIElement.CloseElement();
        }
        

    }
    public void PickupACoin()
    {
        Coins++;
        CoinCounter.Singleton.LoadCoinData();
    }
    public void DisplayA()
    {

    }
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
            StartCoroutine(DoRoll(2, transform.position, telepos,false));

            IsInRoll = true;
            canRoll = false;
            Invoke(nameof(ResetRoll), 1);
            
        }
    }
    public IEnumerator DoRoll(float speed,Vector2 initpos,Vector2 telepos,bool IsIntro)
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
        if (IsIntro)
            IsInIntro = false;
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (Grounded && canJump&&!IsInRoll&&!IsInDialoge)
        {
            canJump = false;
            Invoke(nameof(ResetJump), 0.8f);
            rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
        }

    }

    public void OnWalk(Vector2 wasd)
    {
        if (IsInRoll || IsInDialoge) return;
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
        actions.FindActionMap("GunPlay").Enable();
        actions.FindActionMap("UI").Enable();
    }
    void OnDisable()
    {
        actions.FindActionMap("MoveMent").Disable();
    }

    public override void Die()
    {
        
    }
}

