using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Events;
using TriInspector;
using DomainLogging;

[System.Serializable]
public abstract class BaseState
{
    public event UnityAction<bool> StateEnded;

    [LabelWidth(100)]
    [FormerlySerializedAs("HasTime")]
    [SerializeField] bool m_HasTime = true;

    [ShowIf(nameof(m_HasTime))]
    [LabelWidth(100)]
    [FormerlySerializedAs("IsExitTime")]
    [SerializeField] bool m_IsExitTime = true;

    [ShowIf(nameof(m_HasTime))]
    [LabelWidth(100)]
    [FormerlySerializedAs("Time")]
    [Unit(UnitAttribute.Second)]
    [SerializeField, Min(0)] float m_Time = 10;

    bool m_Ended = false;
    Timer m_Timer;
    protected Enemy p_Enemy;

    void OnTimerEnded()
    {
        DomainDebug.Log($"{GetType()}: timer state ended", DomainType.State);
        m_Ended = m_IsExitTime;
        StateEnded?.Invoke(!m_IsExitTime);
    }

    protected void StateEnd()
    {
        DomainDebug.Log($"{GetType()}: state ended", DomainType.State);
        m_Ended = true;
        StateEnded?.Invoke(false);
    }

    public void Enter(Enemy enemy)
    {
        m_Ended = false;
        DomainDebug.Log($"{enemy.name} entered in {GetType()}", DomainType.State);
        if (m_HasTime)
        {
            m_Timer = new Timer(m_Time);
            m_Timer.TimerEnded += OnTimerEnded;
        }

        p_Enemy = enemy;
        OnEnter();
    }
    protected virtual void OnEnter() { }

    public void Update(float dt)
    {
        if (m_Ended) return;
        if (m_HasTime) m_Timer.Update(dt);
        OnUpdate(dt);
    }
    protected virtual void OnUpdate(float dt) { }

    public void Exit()
    {
        DomainDebug.Log($"{p_Enemy.name} exited from {GetType()}", DomainType.State);
        OnExit();
        if (p_Enemy.NavAgent.enabled && p_Enemy.NavAgent.isOnNavMesh)
            p_Enemy.NavAgent.ResetPath();

        p_Enemy.Animator.ResetAllTriggers();
        p_Enemy = null;
        StateEnded = null;
        m_Timer = null;
    }
    protected virtual void OnExit() { }

    public void Continue()
    {
        DomainDebug.Log($"{p_Enemy.name} continued {GetType()}", DomainType.State);
        m_Ended = false;
        if (m_HasTime)
        {
            m_Timer.Reset();
            m_Timer.Activate();
        }

        OnContinue();
    }
    protected virtual void OnContinue() { }

    public virtual BaseState Clone() => (BaseState)MemberwiseClone();
}
