using UnityEngine;

public class EnemyAnimator : MonoBehaviour, IAnimator
{
    public event UnityEngine.Events.UnityAction Attacked;

    [SerializeField] Animator m_Animator;

    public void ResetAllTriggers()
    {
        System.Collections.Generic.List<string> triggers = new() { "Attack", "Idle", "Walk", "Run", "Death" };
        foreach (var trig in triggers)
            m_Animator?.ResetTrigger(trig);
    }

    bool IsPlaying(string trigger)
    {
        var clips = m_Animator.GetCurrentAnimatorClipInfo(0);
        if (clips.Length == 0) return false;
        return clips[0].clip.name.ToLower().Contains(trigger.ToLower());
    }

    void Play(string trigger)
    {
        var playing = IsPlaying(trigger);
        // Debug.Log($"Playing: {trigger}, is playing: {playing}");
        if (!playing)
            m_Animator?.SetTrigger(trigger);
        else m_Animator?.ResetTrigger(trigger);
    }

    public void Attack()
    {
        Play("Attack");
		if (m_Animator == null) OnStateExited(true);
    }

    public void Idle()
    {
        Play("Idle");
    }

    public void Walk()
    {
        Play("Walk");
    }

    public void Run()
    {
        Play("Run");
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
