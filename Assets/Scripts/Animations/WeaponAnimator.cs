using UnityEngine;

public class WeaponAnimator : MonoBehaviour, IAnimator, IWeaponActions
{
    public event UnityEngine.Events.UnityAction<bool> OnUneqiuped;

    [SerializeField] Animator m_Animator;

    public void Attack()
    {
        if (m_Animator == null) return;
        m_Animator.SetTrigger("Attack");
    }

    public void Reload()
    {
        if (m_Animator == null) return;
        m_Animator.SetTrigger("Reload");
    }

    public void Equip()
    {
        if (m_Animator == null) return;
        m_Animator.SetTrigger("Equip");
    }

    public void Unequip()
    {
        if (m_Animator == null) return;
        m_Animator.SetTrigger("Hide");
    }

    void OnHideExited(bool finished)
    {
        OnUneqiuped?.Invoke(finished);
        OnUneqiuped = null;
    }

    public void OnStateExited(bool stateFinished)
    {
        OnHideExited(stateFinished);
    }
}
