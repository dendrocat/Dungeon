using UnityEngine;

[RequireComponent(typeof(Collider), typeof(BonusAnimator), typeof(BonusSound))]
public abstract class Bonus : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject m_Prefab;
    [SerializeField] Transform m_SpawnPos;
    BonusAnimator m_Animator;
    BonusSound m_Sound;

    bool m_Interacted = false;

    GameObject m_Obj;
    void Awake()
    {
        m_Obj = Instantiate(m_Prefab, m_SpawnPos.position, m_SpawnPos.rotation, transform);
        m_Animator = GetComponent<BonusAnimator>();
        m_Animator.Hided += () => Destroy(gameObject);

        m_Sound = GetComponent<BonusSound>();
    }

    public void Interact(Player player)
    {
        if (m_Interacted) return;
        m_Interacted = true;
        OnInteract(player);
        m_Animator.Hide();
        m_Sound.Play();
        Destroy(m_Obj);
    }

    protected abstract void OnInteract(Player player);
}
