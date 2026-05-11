using UnityEngine;
using UnityEngine.Events;

public class WeaponAnimator : MonoBehaviour
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
		StateExitor.StateExited += OnHideExited;
        if (!m_HasTriggers) return;
        m_Animator.SetTrigger("Hide");
    }

    void OnHideExited(bool finished)
    {
		StateExitor.StateExited -= OnHideExited;
        OnUneqiped?.Invoke(finished);
        OnUneqiped = null;
    }

}
