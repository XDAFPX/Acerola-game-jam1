using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public bool HasGasMask;
    public bool Grounded { get { if (this == null) return false; if (Physics2D.BoxCast(transform.position, new Vector2(1,1f),0,Vector2.down,1f,Ground)) { return true; } return false; } }
    [HideInInspector]public Rigidbody2D rb;
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
    public bool IsInBallPit;
    public ParticleSystem BallPitParticles;
    public ParticleSystem JumpParticles;
    public Transform[] StepingRaySpots;
    public float SteppingJump = 0.125f;
    [HideInInspector] public bool CanSwitchWeapons = true;
    [HideInInspector]public Animator grapchics;
    [HideInInspector] public BaseGun Activegun;
    public bool IsStendingOnSlipperyFloor { get { return (Physics2D.BoxCast(transform.position, new Vector2(1, 1f), 0, Vector2.down, 1f, Ground).collider.tag == "Slippery"); } }
    void Awake()
    {
        actions.FindActionMap("MoveMent").FindAction("Jump").performed += OnJump;
        actions.FindActionMap("MoveMent").FindAction("Roll").performed += OnRoll;
        actions.FindActionMap("UI").FindAction("Close").performed += OnEsc;
        actions.FindActionMap("UI").FindAction("Inventory").performed += OnTab;
        moveAction = actions.FindActionMap("MoveMent").FindAction("WASD");
        moveAction.performed += WalkPerfarmed;
        //DontDestroyOnLoad(this.gameObject);
    }

    private void WalkPerfarmed(InputAction.CallbackContext obj)
    {

        if(IsInBallPit)
            SpawnBallPitParticle();
    }

   
    public void SetSpeed(float value) { Speed = value; }
    public void SetJumpPower(float value) { JumpPower = value; }
    public void SetInBallPit(bool value) { IsInBallPit = value; }
    public void SpawnBallPitParticle() { Instantiate(BallPitParticles, transform.position, Quaternion.identity); }

    public void ResetJump() { canJump = true; }
    public void ResetRoll() { canRoll = true; }

    private void Start()
    {
        ShowCursor = false;
        rb = GetComponent<Rigidbody2D>();
        grapchics = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        CanSwitchWeapons = true;
        if (IsInIntro)
        {
            StartCoroutine(DoRoll(0.5f, transform.position, transform.position + transform.right * 3,true));
        }
        CoinCounter.Singleton.LoadCoinData();
    }
    public void FixedUpdate()
    {
        OnWalk(moveAction.ReadValue<Vector2>());
        grapchics.SetBool("IsGrounded", Grounded);

    }
    public override void HealDamage(float value)
    {

        base.HealDamage(value);
        PostProcesingManager.Singleton.SetDamagedPost(Mathf.Abs(1 - (Hp / 100)));
    }
    public override void TakeDamage(float value)
    {
        CameraShaker.Singleton.StartShake(10, 1, 0.3f);
        base.TakeDamage(value);
        PostProcesingManager.Singleton.SetDamagedPost(Mathf.Abs(1 - (Hp / 100)));
    }
    private void TryStep(Vector2 flatwasd)
    {
        if (flatwasd.x == 0) return;
        Vector2 lowstepPos;
        Vector2 highstepPos;
        if(flatwasd.x == 1)
        {
            lowstepPos = StepingRaySpots[0].position;
            highstepPos = StepingRaySpots[1].position;
        }
        else
        {
            lowstepPos = StepingRaySpots[2].position;
            highstepPos = StepingRaySpots[3].position;
        }

        if (Physics2D.Raycast(lowstepPos, flatwasd, 0.1f, Ground))
        {
            if(!Physics2D.Raycast(highstepPos, flatwasd, 0.2f, Ground))
            {
                rb.position += new Vector2(0, SteppingJump);
                rb.velocity += flatwasd*1f;
            }
        }
    }
    public void AdvanceQuest1()
    {
        GameObject.Find("DialogGuy").GetComponent<Dialogtrigger>().ProgressDialog(1);
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
        if (Input.anyKeyDown&&IsDead)
        {
            if (SaveNLoadManager.Singleton != null)
                SaveNLoadManager.Singleton.Load();
            else SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    public void SwitchWeapon()
    {
        if(weapon == 1&&HasPistol&CanSwitchWeapons)
        {
            Magnum.gameObject.SetActive(true);
            Activegun = Magnum;
            Shotgun.gameObject.SetActive(false);
        }
        if (weapon == 2 && HasShotgun&&CanSwitchWeapons)
        {
            Magnum.gameObject.SetActive(false);
            Shotgun.gameObject.SetActive(true);
            Activegun = Shotgun;
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
        if (IsInDialoge&&IsDead) return;
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
        else
        {
            ShowCursor = !ShowCursor;
            GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("Menu").gameObject.SetActive(!GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("Menu").gameObject.activeInHierarchy);
        }

    }
    public void RevolverAmmoPickup()
    {
        PistolAmmo += 30;
    }
    public void ShotgunShelsPickup()
    {
        ShotgunAmmo += 15;
    }
    public void SyringeRegen()
    {
        HealDamage(40);
    }
    public void FirtaAidRegen()
    {
        HealDamage(100);
    }
    public void PickupACoin()
    {
        Coins++;
        CoinCounter.Singleton.LoadCoinData();
    }
    public void DisplayA()
    {

    }
    public void PickupRevolver()
    {
        HasPistol = true;
        weapon = 1;
        SwitchWeapon();
        AmmoUI.Singleton.Show();
        HintUI.Singleton.ShowHint("KILL KILL KILL KILL KILL");
    }
    public void PickupShotgun()
    {
        //Debug.Log("ddd");
        HasShotgun = true;
        weapon = 2;
        SwitchWeapon();
        AmmoUI.Singleton.Show();
    }
    public void OnE(InputAction.CallbackContext context)
    {
        if (IsInDialoge) return;

    }
    public void OnRoll(InputAction.CallbackContext context)
    {
        if (canRoll && Grounded&&!IsInBallPit&&!IsInDialoge&&!IsDead)
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
            if (IsInBallPit)
                SpawnBallPitParticle();
            
        }
    }
    public IEnumerator DoRoll(float speed,Vector2 initpos,Vector2 telepos,bool IsIntro)
    {
        grapchics.Play("BodyRoll");
        float time = 0;

        while (time < 1)
        {
            float t = InOutQuint(time);
            transform.position = Vector2.Lerp(initpos, telepos, t);

            time += Time.deltaTime * speed;
            yield return null;
        }
        IsInRoll = false;
        //if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "P1_a1")
            //Instantiate(JumpParticles, transform.position, Quaternion.identity);
        if (IsIntro)
            IsInIntro = false;
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (this == null) return;
        if (Grounded && canJump&&!IsInRoll&&!IsInDialoge&&!IsDead)
        {
            grapchics.Play("BodyJump");
            if (IsInBallPit)
                SpawnBallPitParticle();
            else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "P1_a1")
            {
                var hit = Physics2D.BoxCast(transform.position, new Vector2(1, 1f), 0, Vector2.down, 2f, Ground);
                Instantiate(JumpParticles, hit.point, Quaternion.identity);
            }
            canJump = false;
            Invoke(nameof(ResetJump), 0.8f);
            rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
        }

    }

    public void OnWalk(Vector2 wasd)
    {
        float wdir = 1;
        if (wasd.x > 0) wdir=1;
        if (wasd.x < 0) wdir=-1;
        float scalex;
        if (Activegun == null)
            scalex = wdir;
        else
        {
            if (wdir == Activegun.preferebledir)
                scalex = wdir;
            else
            {
                scalex = Activegun.preferebledir;
            }
        }
        grapchics.transform.parent.localScale = new Vector3(scalex, 1, 1);

        if (IsInRoll || IsInDialoge || IsDead) return;
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
        if(flatwasd.x == 0&& Grounded&&!IsStendingOnSlipperyFloor)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        grapchics.SetBool("IsRunning", (flatwasd.x != 0));
        TryStep(flatwasd);
        if(Mathf.Abs(rb.velocity.x) > Speed+20)
        {
            rb.AddForce(flatwasd * -10);
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
        GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("DeadScreen").gameObject.SetActive(true);
    }
}

