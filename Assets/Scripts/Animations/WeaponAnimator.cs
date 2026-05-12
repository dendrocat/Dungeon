using UnityEngine;
using UnityEngine.Events;

public class WeaponAnimator : MonoBehaviour, IAnimator
{
    public event UnityAction<bool> OnUneqiped;

    [SerializeField] Animator m_Animator;
    [SerializeField] bool m_HasTriggers = true;

    public void Attack()
    {
        if (!m_HasTriggers) return;
        m_Animator.SetTrigger("Attack");
    }

    public void Reload()
    {
        if (!m_HasTriggers) return;
        m_Animator.SetTrigger("Reload");
    }

    public virtual void Equip()
    {
        m_Animator.SetTrigger("Equip");
    }

    public void Unequip()
    {
        if (!m_HasTriggers) return;
        m_Animator.SetTrigger("Hide");
    }

    void OnHideExited(bool finished)
    {
        OnUneqiped?.Invoke(finished);
        OnUneqiped = null;
    }

    public void OnStateExited(bool stateFinished)
    {
        OnHideExited(stateFinished);
    }
}
