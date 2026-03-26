using UnityEngine;
using DomainLogging;

public abstract class Weapon<TStats> : IWeapon where TStats : WeaponStats
{
    protected TStats p_Stats;
    protected GameObject p_GObj;

    Timer m_ReloadTimer;
    public float ReloadProgress => m_ReloadTimer.Progress;
    public bool IsReloading => m_ReloadTimer.IsActive;

    public bool Equiped { get; private set; } = false;

    public Weapon(in TStats stats, in Transform parent)
    {
        p_Stats = stats;
        p_GObj = Object.Instantiate(p_Stats.WeaponPrefab, parent);
        p_GObj.SetActive(false);

        m_ReloadTimer = new Timer(p_Stats.ReloadTime, false);
        m_ReloadTimer.TimerEnded += AfterReload;
    }

    protected virtual bool CanAttack()
    {
        return !IsReloading;
    }

    public bool Attack(Vector3? target = null)
    {
        if (!CanAttack()) return false;
        OnAttack(target);
        return true;
    }
    protected virtual void OnAttack(Vector3? target = null) { }

    protected virtual bool CanReload()
    {
        return !IsReloading;
    }

    public void Reload()
    {
        if (!CanReload()) return;
        DomainDebug.Log($"{p_Stats.name} reloading...", DomainType.Weapon);
        m_ReloadTimer.Activate();
        m_ReloadTimer.Reset();
    }

    protected virtual void AfterReload()
    { }

    public void Update(float dt)
    {
        m_ReloadTimer.Update(dt);
        OnUpdate(dt);
    }

    protected virtual void OnUpdate(float dt)
    { }

    public virtual void Equip()
    {
        p_GObj.SetActive(true);
        Equiped = true;
        // DomainDebug.Log($"{p_GObj.name} equiped", DomainType.Weapon);
    }

    public void Unequip(bool destroy = true)
    {
        if (destroy) Object.Destroy(p_GObj);
        else p_GObj.SetActive(false);

        Equiped = false;
        // DomainDebug.Log($"{p_GObj.name} unequiped", DomainType.Weapon);
    }
}
