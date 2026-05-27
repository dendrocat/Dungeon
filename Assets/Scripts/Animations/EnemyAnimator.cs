using UnityEngine;

public class EnemyAnimator : MonoBehaviour, IAnimator
{
    public event UnityEngine.Events.UnityAction Attacked;

    Animator m_Animator;

    void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
    }

    bool IsPlaying(string trigger)
    {
        var state = m_Animator?.GetCurrentAnimatorStateInfo(0);
        if (!state.HasValue || state.Value.length == 0) return false;
        return state.Value.IsName(trigger);
    }

    void Play(string trigger)
    {
        var playing = IsPlaying(trigger);
        // Debug.Log($"Playing: {trigger}, is playing: {playing}");
        if (!playing)
            m_Animator?.SetTrigger(trigger);
        else m_Animator?.ResetTrigger(trigger);
    }

    void Play(int move)
    {
        m_Animator?.SetInteger("Move", move);
    }

    public void Attack()
    {
        Play("Attack");
        if (m_Animator == null) OnStateExited(true);
    }

    public void Idle()
    {
        // Play("Idle");
        Play(0);
    }

    public void Walk()
    {
        // Play("Walk");
        Play(1);
    }

    public void Run()
    {
        // Play("Run");
        Play(2);
    }

    public void Die()
    {
        Play("Death");
    }

    public void OnStateExited(bool stateFinished)
    {
        Attacked?.Invoke();
    }
}
