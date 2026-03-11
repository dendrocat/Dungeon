using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Events;
using System;
using TriInspector;

[Serializable]
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
    [SerializeField, Min(0), Unit(UnitAttribute.Second)] float m_Time = 10;

	bool m_Ended = false;
    Timer m_Timer;
    protected Enemy p_Enemy;

    void OnTimerEnded()
    {
        Debug.Log($"{GetType()}: Timer ended");
		m_Ended = m_IsExitTime;
        StateEnded?.Invoke(!m_IsExitTime);
    }

	protected void StateEnd() {
		Debug.Log($"{GetType()}: state ended");
		m_Ended = true;
		StateEnded?.Invoke(false);
	}

    public void Enter(Enemy enemy)
    {
		m_Ended = false;
        Debug.Log($"{enemy.name} entered in {GetType()}");
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
        Debug.Log($"{p_Enemy.name} exited from {GetType()}");
        OnExit();

        p_Enemy = null;
        StateEnded = null;
        m_Timer = null;
    }
    protected virtual void OnExit() { }

    public void Continue()
    {
        Debug.Log($"{p_Enemy.name} continued {GetType()}");
        if (m_HasTime) m_Timer.Reset();

        OnContinue();
    }
    protected virtual void OnContinue() { }
}
