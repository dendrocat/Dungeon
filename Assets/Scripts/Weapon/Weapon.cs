using UnityEngine;

public abstract class Weapon<TStats> : IWeapon where TStats : WeaponStats
{
    protected TStats p_Stats;
    protected GameObject p_GObj;

    Timer m_ReloadTimer;
    public float ReloadProgress => m_ReloadTimer.Progress;
    public bool IsReloading { get; private set; } = false;

    public bool Equiped { get; private set; } = false;

    public Weapon(in TStats stats, in Transform parent)
    {
        p_Stats = stats;
        p_GObj = Object.Instantiate(p_Stats.WeaponPrefab, parent);
        p_GObj.SetActive(false);

        m_ReloadTimer = new Timer(p_Stats.ReloadTime);
        m_ReloadTimer.TimerEnded += AfterReload;
    }

    protected virtual bool CanAttack()
    {
        return !IsReloading;
    }

    public void Attack(Vector3? target = null)
    {
        if (!CanAttack()) return;
        OnAttack(target);
    }
    protected virtual void OnAttack(Vector3? target = null) { }

    protected virtual bool CanReload()
    {
        return !IsReloading;
    }

    public void Reload()
    {
        if (!CanReload()) return;
        Debug.Log($"{p_Stats.name} reloading...");
        IsReloading = true;
        m_ReloadTimer.Reset();
    }

    protected virtual void AfterReload() { IsReloading = false; }

    public virtual void OnUpdate()
    {
        if (IsReloading)
            m_ReloadTimer.Update(Time.deltaTime);
    }

    public virtual void Equip()
    {
        p_GObj.SetActive(true);
        Equiped = true;
        // Debug.Log($"{p_GObj.name} equiped");
    }

    public void Unequip(bool destroy = true)
    {
        if (destroy) Object.Destroy(p_GObj);
        else p_GObj.SetActive(false);

        Equiped = false;
        // Debug.Log($"{p_GObj.name} unequiped");
    }
}
