using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DomainLogging;

#if UNITY_EDITOR
using TriInspector;

[DeclareBoxGroup("insp", Title = "DomainDebug View")]
#endif
public class StateMachine : MonoBehaviour
{
    public event UnityAction ChangeStateRequested;

    [SerializeField] Enemy m_Enemy;
    [SerializeField] StatesConfig m_Config;

    StatesConfig.StateEntry m_ActiveState;

    bool m_CanActiveState = false;
    bool[] m_ExitStates = new bool[Enum.GetValues(typeof(States)).Length];

#if UNITY_EDITOR
    [Group("insp")]
    [ShowInInspector]
    States ActiveState => m_ActiveState?.State ?? States.Idle;
#endif

    void UpdateExitStates()
    {
        List<States> exits = m_Config.GetExitStates(m_ActiveState.State);
        foreach (var state in exits)
        {
            m_ExitStates[(int)state] = true;
        }
    }

    void ActivateState()
    {
        m_ActiveState.MachineState.Enter(m_Enemy);
        m_ActiveState.MachineState.StateEnded += OnStateEnded;

        UpdateExitStates();
    }

    void Start()
    {
        m_ActiveState = m_Config.GetStartState();
        ActivateState();
    }

    public int GetActiveState()
    {
        return (int)m_ActiveState.State;
    }

    public IReadOnlyList<bool> GetExitStates()
    {
        m_ExitStates[GetActiveState()] = m_CanActiveState;
        return m_ExitStates;
    }

    public bool ChangeState(int state)
    {
        DomainDebug.Log($"Trying change state to {state}", DomainType.StateMachine);
        if (!Enum.IsDefined(typeof(States), state)) return false;
        DomainDebug.Log($"Changing state to {(States)state}", DomainType.StateMachine);
        if ((States)state == m_ActiveState.State)
        {
            m_ActiveState.MachineState.Continue();
            return true;
        }

        m_ActiveState.MachineState.Exit();

        m_ActiveState = m_Config.GetState((States)state);

        ActivateState();
        return true;
    }

    void OnStateEnded(bool canActive)
    {
        DomainDebug.Log($"{m_ActiveState.State} ended. This state can be active {canActive}. Request next state", DomainType.StateMachine);
        m_CanActiveState = canActive;
        ChangeStateRequested.Invoke();
    }

#if UNITY_EDITOR
    [Group("insp")]
    [PropertySpace(20)]
    [Button(ButtonSizes.Small)]
    void ForceStateEnd(bool canActive)
    {
        OnStateEnded(canActive);
    }
#endif

    void Update()
    {
        m_ActiveState.MachineState.Update(Time.deltaTime);
    }
}
