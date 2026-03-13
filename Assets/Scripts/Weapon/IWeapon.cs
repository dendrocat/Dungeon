public interface IWeapon
{
    public float ReloadProgress { get; }
    public bool IsReloading { get; }
    public bool Equiped { get; }

    void Attack(UnityEngine.Vector3? target = null);
    void Reload();

    void OnUpdate();

    void Equip();
    void Unequip(bool destroy);
}
