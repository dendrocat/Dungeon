using UnityEngine;

[RequireComponent(typeof(MouseLook), typeof(PlayerMover))]
public class Player : Person
{
    public Health Health => p_Health;

	[SerializeField] PlayerWeaponHandler m_WeaponHandler;

    void Start()
    {
		if (!m_WeaponHandler.IsActive) return;
        InputManager.Instance.WeaponNumed += m_WeaponHandler.ChangeWeapon;
        InputManager.Instance.Reloaded += m_WeaponHandler.Reload;
        InputManager.Instance.Throwed += m_WeaponHandler.ThrowGrenade;

        m_WeaponHandler.ChangeWeapon(1);
    }

	void Update() {
		if (!m_WeaponHandler.IsActive) return;
        if (InputManager.Instance.Attack) m_WeaponHandler.Attack();
	}
}

