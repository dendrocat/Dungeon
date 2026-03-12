public interface IWeapon
{
    public float ReloadProgress { get; }
    public bool IsReloading { get; }

    void Attack();
    void Reload();

    void OnUpdate();

    void Equip();
    void Unequip(bool destroy);
}
