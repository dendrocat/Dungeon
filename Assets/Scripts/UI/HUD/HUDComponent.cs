using UnityEngine;

public abstract class HUDComponent : MonoBehaviour
{
    public abstract void Init(Player player);

    protected abstract void Update();
}
