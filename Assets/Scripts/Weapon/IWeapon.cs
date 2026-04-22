public interface IWeapon
{
    public WeaponType Type { get; }

    public float ReloadProgress { get; }
    public bool IsReloading { get; }
    public bool Equiped { get; }

    bool Attack(UnityEngine.Vector3? target = null);
    void Reload();

    void Update(float dt);

    void Equip();
    void Unequip(bool destroy = false);
}
