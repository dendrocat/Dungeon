using UnityEngine;

public class WeaponHandler : MonoBehaviour, IActivatable
{
    [SerializeField] WeaponStats m_WeaponStats;

    public bool IsActive => enabled;
    IWeapon m_Weapon;

    void Awake()
    {
        if (m_WeaponStats is RangedWeaponStats stats)
            m_Weapon = new RangedWeapon(stats, transform);
		else m_Weapon = new MeleeWeapon(m_WeaponStats, transform);
    }

    void Start()
    {
    }

    void Attack()
    {
        m_Weapon.Attack();
    }

    void Reload()
    {
        m_Weapon.Reload();
    }

	void Update() {
		m_Weapon.OnUpdate();	
	}

    public void Activate() => enabled = true;
    public void Deactivate() => enabled = false;
}
