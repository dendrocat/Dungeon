using UnityEngine;

public class FinishAnimator : MonoBehaviour, IAnimator
{
    public UnityEngine.Events.UnityAction Opened;

    [SerializeField] Animator m_Animator;

    public void OnStateExited(bool stateFinished)
    {
        Opened?.Invoke();
    }

    public void Open()
    {
        m_Animator.SetTrigger("Open");
    }
}
