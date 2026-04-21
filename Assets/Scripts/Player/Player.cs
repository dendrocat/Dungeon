using UnityEngine;
// using DomainLogging;

[RequireComponent(typeof(MouseLook), typeof(PlayerMover))]
public class Player : Person<PlayerConfig, PlayerHealth, PlayerHealthConfig>
{
    [TriInspector.Group("cmp")]
    [SerializeField] PlayerWeaponHandler m_WeaponHandler;

	IInput m_Input;
	public IInput Input => m_Input;

    int m_LightZoneCount = 0;
    public bool IsLighted => m_LightZoneCount > 0;

    protected override void OnAwake()
    {
		base.OnAwake();
		m_Input = GetComponentInChildren<IInput>();	
		GetComponent<PlayerMover>().Init(Config);
	}

    void Start()
    {
        Person.Died += OnEnemyDied;

        if (!m_WeaponHandler.IsActive) return;
        Input.WeaponNumed += m_WeaponHandler.ChangeWeapon;
        Input.Reloaded += m_WeaponHandler.Reload;
        Input.Throwed += m_WeaponHandler.ThrowGrenade;
        Input.MeleeAttacked += m_WeaponHandler.MeleeAttack;

        m_WeaponHandler.ChangeWeapon(1);
    }
    void OnEnable()
    {
        LightZone.PlayerInsideChanged += OnLightZonePlayerInsideChanged;
    }

    void OnDisable()
    {
        LightZone.PlayerInsideChanged -= OnLightZonePlayerInsideChanged;
    }

    void OnLightZonePlayerInsideChanged(bool playerInside)
    {
        m_LightZoneCount += playerInside ? 1 : -1;
        // DomainDebug.Log($"Player lighted: {IsLighted}", DomainType.Player);
    }

    void OnEnemyDied(Person p)
    {
        if (p is not Enemy) return;
        m_WeaponHandler.AddAmmo();
    }

    void Update()
    {
        if (!m_WeaponHandler.IsActive) return;
        if (Input.Attack) m_WeaponHandler.Attack();
    }
}

