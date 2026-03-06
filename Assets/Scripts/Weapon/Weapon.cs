using UnityEngine;

public abstract class Weapon<TStats> where TStats : WeaponStats
{
    protected TStats p_Stats;
    protected GameObject p_GObj;

    public float ReloadTimer { get; private set; }
    public int ReloadTime => p_Stats.ReloadTime;
    public bool Reloading => ReloadTimer < p_Stats.ReloadTime;

    public bool Equiped { get; private set; } = false;

    public Weapon(in TStats stats, in Transform parent)
    {
        p_Stats = stats;
        ReloadTimer = p_Stats.ReloadTime;
        p_GObj = Object.Instantiate(p_Stats.WeaponPrefab, parent);
        p_GObj.SetActive(false);
    }

    protected virtual bool CanAttack()
    {
        return !Reloading;
    }

    public void Attack()
    {
        if (!CanAttack()) return;
        OnAttack();
    }
    protected abstract void OnAttack();

    protected virtual bool CanReload()
    {
        return !Reloading;
    }

    public void Reload()
    {
        if (!CanReload()) return;
        Debug.Log($"{p_Stats.name} reloading...");
        ReloadTimer = 0;
    }

    protected abstract void AfterReload();

    public virtual void OnUpdate()
    {
        if (Reloading)
        {
            ReloadTimer += Time.deltaTime;
            if (!Reloading) AfterReload();
        }
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
