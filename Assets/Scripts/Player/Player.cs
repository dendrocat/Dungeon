using UnityEngine;

[RequireComponent(typeof(MouseLook), typeof(PlayerMover))]
public class Player : Person
{
	[TriInspector.Group("cmp")]
    [SerializeField] PlayerWeaponHandler m_WeaponHandler;

    void Start()
    {
        if (!m_WeaponHandler.IsActive) return;
		IInput.Instance.WeaponNumed += m_WeaponHandler.ChangeWeapon;
        IInput.Instance.Reloaded += m_WeaponHandler.Reload;
        IInput.Instance.Throwed += m_WeaponHandler.ThrowGrenade;
        IInput.Instance.MeleeAttacked += m_WeaponHandler.MeleeAttack;

        m_WeaponHandler.ChangeWeapon(1);
    }

    void Update()
    {
        if (!m_WeaponHandler.IsActive) return;
        if (IInput.Instance.Attack) m_WeaponHandler.Attack();
    }
}

