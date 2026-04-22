using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Bonus : MonoBehaviour, IInteractable
{
    protected abstract GameObject p_Prefab { get; }
    [SerializeField] Transform m_SpawnPos;

    bool m_Interacted = false;

	GameObject m_Obj;
    void Awake()
    {
        m_Obj = Instantiate(p_Prefab, m_SpawnPos.position, m_SpawnPos.rotation, transform);
    }

    public void Interact(Player player)
    {
		if (m_Interacted) return;
		m_Interacted = true;
		OnInteract(player);
		Destroy(m_Obj);
    }

    protected abstract void OnInteract(Player player);
}
