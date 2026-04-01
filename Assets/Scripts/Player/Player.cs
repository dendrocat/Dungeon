using DomainLogging;
using UnityEngine;

[RequireComponent(typeof(MouseLook), typeof(PlayerMover))]
public class Player : Person
{
    [TriInspector.Group("cmp")]
    [SerializeField] PlayerWeaponHandler m_WeaponHandler;

    int m_LightZoneCount = 0;
    public bool IsLighted => m_LightZoneCount > 0;

    void Start()
    {
        if (!m_WeaponHandler.IsActive) return;
        IInput.Instance.WeaponNumed += m_WeaponHandler.ChangeWeapon;
        IInput.Instance.Reloaded += m_WeaponHandler.Reload;
        IInput.Instance.Throwed += m_WeaponHandler.ThrowGrenade;
        IInput.Instance.MeleeAttacked += m_WeaponHandler.MeleeAttack;
        Person.Died += OnEnemyDied;

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
        DomainDebug.Log($"Player lighted: {IsLighted}", DomainType.Player);
    }

    void OnEnemyDied(Person p)
    {
        if (p is not Enemy) return;
        m_WeaponHandler.AddAmmo();
    }

    void Update()
    {
        if (!m_WeaponHandler.IsActive) return;
        if (IInput.Instance.Attack) m_WeaponHandler.Attack();
    }
}

