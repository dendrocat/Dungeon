using UnityEngine;

public class BonusAnimator : MonoBehaviour, IAnimator
{
    public event UnityEngine.Events.UnityAction Hided;

    [SerializeField] Animator m_Animator;
    public void Hide()
    {
        m_Animator.SetTrigger("Hide");
    }

    public void OnStateExited(bool stateFinished)
    {
		Hided?.Invoke();
    }
}
