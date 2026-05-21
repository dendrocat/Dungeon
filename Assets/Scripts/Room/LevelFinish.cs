using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(FinishAnimator))]
public class LevelFinish : MonoBehaviour, IInteractable
{
    public UnityAction StartLevelFinish;
    public UnityAction LevelFinished;

    FinishAnimator m_Animator;

    void Awake()
    {
        m_Animator = GetComponent<FinishAnimator>();
        m_Animator.Opened += () => LevelFinished?.Invoke();
    }

    public void Interact(Player _)
    {
        StartLevelFinish?.Invoke();
        m_Animator.Open();
    }
}
