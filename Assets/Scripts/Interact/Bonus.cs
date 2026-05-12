using UnityEngine;

[RequireComponent(typeof(Collider), typeof(BonusAnimator))]
public abstract class Bonus : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject m_Prefab;
    [SerializeField] Transform m_SpawnPos;
	BonusAnimator m_Animator;

    bool m_Interacted = false;

	GameObject m_Obj;
    void Awake()
    {
        m_Obj = Instantiate(m_Prefab, m_SpawnPos.position, m_SpawnPos.rotation, transform);
		m_Animator = GetComponent<BonusAnimator>();
		m_Animator.Hided += () => Destroy(gameObject);
    }

    public void Interact(Player player)
    {
		if (m_Interacted) return;
		m_Interacted = true;
		OnInteract(player);
		m_Animator.Hide();
		Destroy(m_Obj);
    }

    protected abstract void OnInteract(Player player);
}
