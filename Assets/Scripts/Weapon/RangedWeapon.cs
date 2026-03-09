using UnityEngine;

public class RangedWeapon : Weapon<RangedWeaponStats>
{
    public int Ammo { get; private set; }
    public int MaxAmmo => p_Stats.MaxAmmo;
    public int AmmoInTube { get; private set; }

    float m_FireTimer = 0;
    Transform m_FirePoint;

    public RangedWeapon(in RangedWeaponStats stats, in Transform parent) : base(stats, parent)
    {
        m_FirePoint = p_GObj.transform.FindChildWithTag("FirePoint");

        if (m_FirePoint == null)
            Debug.LogError($"RangedWeapon {p_GObj.name}: fire point not found");
    }


    public void SetAmmo(int ammoInTube, int ammo)
    {
        AmmoInTube = ammoInTube;
        Ammo = ammo;
    }

    protected override bool CanAttack()
    {
        var res = base.CanAttack();
        if (AmmoInTube <= 0)
        {
            if (!Reloading) Reload();
            return false;
        }
        return res && m_FireTimer <= 0;
    }

    Vector3 ApplySpread(Vector3 dir)
    {
        var hSpread = Random.Range(-p_Stats.MaxSpread.x, p_Stats.MaxSpread.x);
        var vSpread = Random.Range(-p_Stats.MaxSpread.y, p_Stats.MaxSpread.y);
        return Quaternion.Euler(hSpread, vSpread, 0) * dir;
    }

    protected override void OnAttack()
    {
        m_FireTimer = 60 / p_Stats.FireRate;
        Ray r = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

        Vector3 target;
        if (Physics.Raycast(r, out var hit, p_Stats.Distance)) target = hit.point;
        else target = r.direction * p_Stats.Distance;

        var baseDir = target - m_FirePoint.position;
        for (int i = 0; i < p_Stats.BulletRate; ++i)
        {
            var dir = ApplySpread(target - m_FirePoint.position);
            var ammo = GameObject.Instantiate(p_Stats.AmmoPrefab, m_FirePoint.position, Quaternion.identity).GetComponent<Ammo>();
            ammo.Init(p_Stats);
            ammo.Launch(dir);
        }
        --AmmoInTube;
        if (AmmoInTube <= 0 && p_Stats.AutoReload) Reload();
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
        m_FireTimer = 0;
        Debug.Log($"{p_Stats.name} reloaded. InTube: {AmmoInTube}, Ammo: {Ammo}");
    }

    public void AddAmmo(int ammo)
    {
        Ammo = Mathf.Min(Ammo + ammo, MaxAmmo);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (m_FireTimer > 0)
            m_FireTimer -= Time.deltaTime;
    }
}
