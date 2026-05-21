using UnityEngine;

[RequireComponent(typeof(WeaponAnimator), typeof(WeaponAudio))]
public class WeaponActionContoller : MonoBehaviour, IWeaponActions
{
    public event UnityEngine.Events.UnityAction<bool> OnUneqiuped;

    WeaponAnimator m_Animator;

    IWeaponActions[] m_Actions;

    [SerializeField] bool m_OneAction = false;
    void Awake()
    {
        m_Animator = GetComponent<WeaponAnimator>();

        m_Actions = new IWeaponActions[2];
        m_Actions[0] = m_Animator;
        m_Actions[1] = GetComponent<WeaponAudio>();
    }

    public void Attack()
    {
        if (m_OneAction) return;
        foreach (var action in m_Actions)
            action.Attack();
    }

    public void Reload()
    {
        if (m_OneAction) return;
        foreach (var action in m_Actions)
            action.Reload();
    }

    public void Equip()
    {
        if (m_OneAction)
            m_Animator.OnUneqiuped += Uneqiuped;
        foreach (var action in m_Actions)
            action.Equip();
    }

    public void Unequip()
    {
        if (m_OneAction) return;
        m_Animator.OnUneqiuped += Uneqiuped;
        foreach (var action in m_Actions)
            action.Unequip();
    }

    void Uneqiuped(bool finished)
    {
        OnUneqiuped?.Invoke(finished);
        OnUneqiuped = null;
    }
}
