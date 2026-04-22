using UnityEngine;
using UnityEngine.Events;
using TriInspector;

public abstract class BaseWeaponHandler : MonoBehaviour, IActivatable
{
    public event UnityAction<IWeapon> Attacked;

    protected virtual string StatsLabel => "Weapon Stats";
    [LabelText("$" + nameof(StatsLabel))]
    [SerializeField] protected WeaponStats p_WeaponStats;

    protected IWeapon p_Weapon;

    public bool IsActive => enabled;
    public void Activate() => enabled = true;
    public void Deactivate() => enabled = false;

    public virtual void Attack()
    {
        if (p_Weapon.Attack()) RaiseAttacked(p_Weapon);
    }
    public virtual void Reload() { p_Weapon.Reload(); }
    protected virtual void FixedUpdate() { p_Weapon.Update(Time.fixedDeltaTime); }

    protected void RaiseAttacked(IWeapon weapon) => Attacked?.Invoke(weapon);
}
