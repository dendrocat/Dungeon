using System;
using System.Collections.Generic;
// using System.Text;
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
        for (int i = 0; i < m_ExitStates.Length; ++i) m_ExitStates[i] = false;
        foreach (var state in exits)
            m_ExitStates[(int)state] = true;
    }

    void ActivateState()
    {
        UpdateExitStates();
        m_ActiveState.MachineState.StateEnded += OnStateEnded;
        m_ActiveState.MachineState.Enter(m_Enemy);
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
        if (m_ExitStates[GetActiveState()])
            m_ExitStates[GetActiveState()] = m_CanActiveState;
        // if (m_ActiveState.State == States.Die)
        // {
        //     StringBuilder s = new();
        //     for (int i = 0; i < m_ExitStates.Length; ++i)
        //     {
        //         s.AppendLine($"{(States)i} can {m_ExitStates[i]}");
        //     }
        //     DomainDebug.Log($"{m_Enemy.name} exit config from {m_ActiveState.State}: {s.ToString()}", DomainType.StateMachine);
        // }
        return m_ExitStates;
    }

    public bool ChangeState(int state)
    {
        // DomainDebug.Log($"{m_Enemy.name}: Trying change state to {state}", DomainType.StateMachine);
        // if (!Enum.IsDefined(typeof(States), state)) return false;
        if ((States)state == m_ActiveState.State)
        {
            m_ActiveState.MachineState.Continue();
            return true;
        }
        DomainDebug.Log($"{transform.parent.name}: Changing state to {(States)state}", DomainType.StateMachine);

        m_ActiveState.MachineState.Exit();

        m_ActiveState = m_Config.GetState((States)state);

        ActivateState();
        return true;
    }

    void OnStateEnded(bool canActive)
    {
        DomainDebug.Log($"{transform.parent.name}: {m_ActiveState.State} ended. This state can be active {canActive}. Request next state", DomainType.StateMachine);
        m_CanActiveState = canActive;
        ChangeStateRequested?.Invoke();
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

    void FixedUpdate()
    {
        // if (m_ActiveState.State == States.Die || m_ActiveState.State == States.Attack)
        //     DomainDebug.Log($"State: {m_ActiveState.State}, Machine: {m_ActiveState.MachineState.GetType()}", DomainType.StateMachine);
        m_ActiveState.MachineState.Update(Time.fixedDeltaTime);
    }

    void OnDestroy()
    {
        m_ActiveState.MachineState.Exit();
    }
}
