using UnityEngine;
using DomainLogging;

public class RangedWeapon : Weapon<RangedWeaponStats>
{
    public int Ammo { get; private set; }
    public int MaxAmmo => p_Stats.MaxAmmo;
    public int AmmoInTube { get; private set; }

    Timer m_FireTimer;
    Transform m_FirePoint;

    public RangedWeapon(in RangedWeaponStats stats, in Transform parent) : base(stats, parent)
    {
        m_FirePoint = p_GObj.transform.FindChildWithTag("FirePoint");

        if (m_FirePoint == null)
            DomainDebug.LogError($"RangedWeapon {p_GObj.name}: fire point not found", DomainType.Weapon);
        m_FireTimer = new Timer(60f / stats.FireRate, false);
    }

    public void SetAmmo(int ammoInTube, int ammo)
    {
        AmmoInTube = Mathf.Min(ammoInTube, p_Stats.MaxAmmoInTube);
        Ammo = Mathf.Min(ammo, p_Stats.MaxAmmo);
    }

    protected override bool CanAttack()
    {
        if (AmmoInTube <= 0)
        {
            if (!IsReloading) Reload();
            return false;
        }
        return base.CanAttack() && !m_FireTimer.IsActive;
    }

    Vector3 ApplySpread(Vector3 dir)
    {
        var hSpread = Random.Range(-p_Stats.MaxSpread.x, p_Stats.MaxSpread.x);
        var vSpread = Random.Range(-p_Stats.MaxSpread.y, p_Stats.MaxSpread.y);
        return Quaternion.Euler(hSpread, vSpread, 0) * dir;
    }

    protected override void OnAttack(Vector3? target = null)
    {
        if (!target.HasValue)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

            if (Physics.Raycast(ray, out var hit, p_Stats.Distance)) target = hit.point;
            else target = ray.direction * p_Stats.Distance;
        }

        var baseDir = (target.Value - m_FirePoint.position).normalized;
        for (int i = 0; i < p_Stats.BulletRate; ++i)
        {
            var dir = ApplySpread(baseDir).normalized;
            var ammo = GameObject.Instantiate(p_Stats.AmmoPrefab, m_FirePoint.position, Quaternion.identity).GetComponent<Ammo>();
            ammo.Init(p_Stats);
            ammo.Launch(dir);
        }
        --AmmoInTube;
        if (AmmoInTube <= 0) Reload();

		m_FireTimer.Activate();
        m_FireTimer.Reset();
    }

    protected override bool CanReload()
    {
        return base.CanReload() && p_Stats.MaxAmmoInTube != AmmoInTube && Ammo > 0;
    }

    protected override void AfterReload()
    {
        var adding = Mathf.Min(Ammo, p_Stats.MaxAmmoInTube - AmmoInTube);
        AmmoInTube += adding;
        Ammo -= adding;
        DomainDebug.Log($"{p_Stats.name} reloaded. InTube: {AmmoInTube}, Ammo: {Ammo}", DomainType.Weapon);

        m_FireTimer.Reset();
    }

    public void AddAmmo(int ammo)
    {
        Ammo = Mathf.Min(Ammo + ammo, MaxAmmo);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        m_FireTimer.Update(Time.deltaTime);
		DomainDebug.Log($"{p_GObj.name}: {m_FireTimer.Progress}", DomainType.Weapon);
    }
}
