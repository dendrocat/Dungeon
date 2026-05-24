using UnityEngine;
// using DomainLogging;

[RequireComponent(typeof(MouseLook), typeof(PlayerMover))]
public class Player : Person<PlayerConfig, PlayerHealth, PlayerHealthConfig>
{
    [TriInspector.Group("cmp")]
    [SerializeField] PlayerWeaponHandler m_WeaponHandler;
    public PlayerWeaponHandler WeaponHandler => m_WeaponHandler;

    IInput m_Input;
    public IInput Input => m_Input;

    int m_LightZoneCount = 0;
    public bool IsLighted => m_LightZoneCount > 0;

    protected override void OnAwake()
    {
        m_Input = GetComponentInChildren<IInput>();
        GetComponent<PlayerMover>().Init(Config);
    }

    void Start()
    {
        if (!m_WeaponHandler.IsActive) return;
        m_WeaponHandler.ChangeWeapon(WeaponType.Pistol);
    }

    void OnEnable()
    {
        Person.Died += OnEnemyDied;

        Input.WeaponNumed += OnWeaponNumed;
        Input.Reloaded += m_WeaponHandler.Reload;
        Input.Throwed += m_WeaponHandler.ThrowGrenade;
        Input.MeleeAttacked += m_WeaponHandler.MeleeAttack;

        LightZone.PlayerInsideChanged += OnLightZonePlayerInsideChanged;
    }

    void OnEnemyDied(Person p)
    {
        if (p is not Enemy) return;
        m_WeaponHandler.AddAmmo();
    }

    void OnWeaponNumed(int weaponNum)
    {
        WeaponType type = weaponNum switch
        {
            1 => WeaponType.Pistol,
            2 => WeaponType.Automat,
            3 => WeaponType.Shotgun,
            _ => WeaponType.None
        };
        m_WeaponHandler.ChangeWeapon(type);
    }

    void OnLightZonePlayerInsideChanged(bool playerInside)
    {
        m_LightZoneCount += playerInside ? 1 : -1;
        // DomainDebug.Log($"Player lighted: {IsLighted}", DomainType.Player);
    }

    void OnDisable()
    {
        Person.Died -= OnEnemyDied;

        Input.WeaponNumed -= OnWeaponNumed;
        Input.Reloaded -= m_WeaponHandler.Reload;
        Input.Throwed -= m_WeaponHandler.ThrowGrenade;
        Input.MeleeAttacked -= m_WeaponHandler.MeleeAttack;

        LightZone.PlayerInsideChanged -= OnLightZonePlayerInsideChanged;
    }

    void FixedUpdate()
    {
        if (!m_WeaponHandler.IsActive) return;
        if (Input.Attack) m_WeaponHandler.Attack();
    }
}

