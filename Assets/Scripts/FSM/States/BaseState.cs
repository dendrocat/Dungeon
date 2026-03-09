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

    void OnTimerEnded()
    {
        Debug.Log($"{GetType()}: Timer ended");
        StateEnded?.Invoke(!p_IsExitTime);
    }

    public void Enter(Enemy enemy)
    {
        Debug.Log($"{enemy.name} entered in {GetType()}");
        m_Timer = new Timer(p_Time);
        m_Timer.TimerEnded += OnTimerEnded;

        p_Enemy = enemy;
        OnEnter();
    }
    protected virtual void OnEnter() { }

    public void Update(float dt)
    {
        m_Timer.Update(dt);
        OnUpdate(dt);
    }
    protected virtual void OnUpdate(float dt) { }

    public void Exit()
    {
        Debug.Log($"{p_Enemy.name} exited from {GetType()}");
        p_Enemy = null;
        StateEnded = null;

        m_Timer = null;
        OnExit();
    }
    protected virtual void OnExit() { }

    public void Continue()
    {
        Debug.Log($"{p_Enemy.name} continued {GetType()}");
        m_Timer.Reset();

        OnContinue();
    }
    protected virtual void OnContinue() { }
}
