public interface IWeapon
{
    public event UnityEngine.Events.UnityAction Unequiped;

    public WeaponStats Stats { get; }

    public float ReloadProgress { get; }
    public bool IsReloading { get; }

    public bool Equiped { get; }
    public bool IsUnequiping { get; }

    bool Attack(UnityEngine.Vector3? target = null);
    void Reload();

    void Update(float dt);

    void Equip();
    void Unequip(bool destroy = false);
}
