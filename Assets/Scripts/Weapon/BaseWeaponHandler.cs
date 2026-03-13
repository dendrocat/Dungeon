using UnityEngine;
using TriInspector;

public abstract class BaseWeaponHandler : MonoBehaviour, IActivatable
{
    protected virtual string StatsLabel => "Weapon Stats";
    [LabelText("$" + nameof(StatsLabel))]
    [SerializeField] protected WeaponStats p_WeaponStats;

    protected IWeapon p_Weapon;

    public bool IsActive => enabled;
    public void Activate() => enabled = true;
    public void Deactivate() => enabled = false;

    public virtual void Attack() { p_Weapon.Attack(); }
    public virtual void Reload() { p_Weapon.Reload(); }
    protected virtual void Update() { p_Weapon.OnUpdate(); }
}
