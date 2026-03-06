using UnityEngine;
using UnityEngine.Events;
using System;
using TriInspector;

[Serializable]
public abstract class BaseState
{
    [HideInInspector] public UnityEvent<bool> StateEnded = new();

    [LabelText("IsExitTime"), LabelWidth(100)]
    [SerializeField] protected bool p_IsExitTime = true;

    [LabelText("Time"), LabelWidth(100)]
    [SerializeField, Min(0), Unit(UnitAttribute.Second)] protected float p_Time = 10;


    Timer m_Timer;

    public BaseState()
    {
		m_Timer = new Timer(p_Time);
		m_Timer.TimerEnded += () => StateEnded.Invoke(!p_IsExitTime);
    }

    public void Enter()
    {
        m_Timer?.Reset();
    }
    public virtual void OnEnter() { }

    public void Update(float dt)
    {
        m_Timer?.Update(dt);
    }
    public virtual void OnUpdate() { }

    public void Exit()
    {
		StateEnded.RemoveAllListeners();
    }
    public virtual void OnExit() { }
}
