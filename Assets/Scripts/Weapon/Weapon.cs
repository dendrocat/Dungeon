using UnityEngine;
using DomainLogging;

public abstract class Weapon<TStats> : IWeapon where TStats : WeaponStats
{
    public event UnityEngine.Events.UnityAction Unequiped;

    protected TStats p_Stats;
    public WeaponStats Stats => p_Stats;

    public TStats GetStats() => Stats as TStats ?? throw new System.InvalidCastException();

    protected GameObject p_GObj;
    WeaponAnimator m_Animator;

    Timer m_ReloadTimer = null, m_AttackTimer = null;
    public float ReloadProgress => m_ReloadTimer.Progress;
    public bool IsReloading => m_ReloadTimer.IsActive;

    public bool Equiped { get; private set; } = false;
    public bool IsUnequiping { get; private set; } = false;

    public Weapon(in TStats stats, in Transform parent)
    {
        p_Stats = stats;
        p_GObj = Object.Instantiate(p_Stats.WeaponPrefab, parent);
        m_Animator = p_GObj.GetComponent<WeaponAnimator>();
        p_GObj.SetActive(false);

        m_ReloadTimer = new Timer(p_Stats.ReloadTime, false);
        m_ReloadTimer.TimerEnded += AfterReload;

        if (stats.UntilAttackTime > 0)
            m_AttackTimer = new Timer(p_Stats.UntilAttackTime, false);
    }

    protected virtual bool CanAttack()
    {
        return !IsReloading;
    }

    public bool Attack(Vector3? target = null)
    {
        void ActivateOnAttack() { OnAttack(target); m_AttackTimer.TimerEnded -= ActivateOnAttack; }

        var can = CanAttack();
        if (can)
        {
            m_Animator?.Attack();
            if (m_AttackTimer != null)
            {
                m_AttackTimer.Reset();
                m_AttackTimer.Activate();
                m_AttackTimer.TimerEnded += ActivateOnAttack;
            }
            else OnAttack(target);
        }
        return can;
    }
    protected virtual void OnAttack(Vector3? target = null) { }

    protected virtual bool CanReload()
    {
        return !IsReloading;
    }

    public void Reload()
    {
        if (!CanReload()) return;
        m_Animator?.Reload();
        DomainDebug.Log($"{p_Stats.name} reloading...", DomainType.Weapon);
        m_ReloadTimer.Activate();
        m_ReloadTimer.Reset();
    }

    protected virtual void AfterReload()
    { }

    public void Update(float dt)
    {
        m_ReloadTimer.Update(dt);
        m_AttackTimer?.Update(dt);
        OnUpdate(dt);
    }

    protected virtual void OnUpdate(float dt)
    { }

    public virtual void Equip()
    {
        p_GObj.SetActive(true);
        Equiped = true;
        m_Animator?.Equip();
        DomainDebug.Log($"{p_GObj.name} equiped", DomainType.Weapon);
    }

    public void Unequip(bool destroy = false)
    {
        if (!Equiped) { OnUnequip(destroy); return; }
        IsUnequiping = true;
        DomainDebug.Log($"{p_GObj.name} unequiping", DomainType.Weapon);
        if (m_Animator != null)
        {
            m_Animator.OnUneqiped += (bool finished) =>
            {
                if (finished)
                {
                    OnUnequip(destroy);
                    Unequiped?.Invoke();
                }
                IsUnequiping = false;
                Unequiped = null;
            };
            m_Animator.Unequip();
        }
        else OnUnequip(destroy);
    }

    void OnUnequip(bool destroy)
    {
        if (destroy) Object.Destroy(p_GObj);
        else p_GObj.SetActive(false);

        // DomainDebug.Log($"{p_GObj.name} unequiped", DomainType.Weapon);
        Equiped = false;
    }
}
