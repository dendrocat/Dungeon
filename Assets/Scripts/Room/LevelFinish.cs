using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(FinishAnimator), typeof(FinishAudio))]
public class LevelFinish : MonoBehaviour, IInteractable
{
    public UnityAction StartLevelFinish;
    public UnityAction LevelFinished;

    FinishAnimator m_Animator;
	FinishAudio m_Audio;

    void Awake()
    {
        m_Animator = GetComponent<FinishAnimator>();
        m_Animator.Opened += () => LevelFinished?.Invoke();

		m_Audio = GetComponent<FinishAudio>();
    }

    public void Interact(Player _)
    {
        StartLevelFinish?.Invoke();
        m_Animator.Open();
		m_Audio.Play();
    }
}
