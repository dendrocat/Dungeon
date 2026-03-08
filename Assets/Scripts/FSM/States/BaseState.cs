using UnityEngine;
using UnityEngine.Events;
using System;
using TriInspector;

[Serializable]
public abstract class BaseState
{
    public event UnityAction<bool> StateEnded;

    [LabelText("IsExitTime"), LabelWidth(100)]
    [SerializeField] protected bool p_IsExitTime = true;

    [LabelText("Time"), LabelWidth(100)]
    [SerializeField, Min(0), Unit(UnitAttribute.Second)] protected float p_Time = 10;


    Timer m_Timer;
    protected Enemy p_Enemy;

    public BaseState()
    {
        m_Timer = new Timer(p_Time);
        m_Timer.TimerEnded += () => StateEnded.Invoke(!p_IsExitTime);
    }

    public void Enter(Enemy enemy)
    {
        p_Enemy = enemy;
        m_Timer?.Reset();
    }
    protected virtual void OnEnter() { }

    public void Update(float dt)
    {
        m_Timer?.Update(dt);
		OnUpdate(dt);
    }
    protected virtual void OnUpdate(float dt) { }

    public void Exit()
    {
        p_Enemy = null;
        StateEnded = null;
    }
    protected virtual void OnExit() { }
}
