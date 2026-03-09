using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

public class StateMachine : MonoBehaviour
{
    public event UnityAction ChangeStateRequested;

    [SerializeField] Enemy m_Enemy;
    [SerializeField] StatesConfig m_Config;

    StatesConfig.StateEntry m_ActiveState;
    bool p_CanActiveState = false;

    void ActivateState()
    {
        m_ActiveState.MachineState.Enter(m_Enemy);
        m_ActiveState.MachineState.StateEnded += OnStateEnded;
    }

    void Awake()
    {
        m_ActiveState = m_Config.GetStartState();
        ActivateState();
    }

    public int GetActiveState()
    {
        return Convert.ToInt32(m_ActiveState.State);
    }

    public IEnumerable<int> GetExitStates()
    {
        List<int> exits = m_Config.GetExitStates(m_ActiveState.State).ConvertAll(e => Convert.ToInt32(e));
        if (!p_CanActiveState)
            exits.Remove(GetActiveState());
        return exits;
    }

    public bool ChangeState(int state)
    {
        Debug.Log($"FSM: Trying change state to {state}");
        if (!Enum.IsDefined(typeof(States), state)) return false;
        Debug.Log($"FSM: Changing state to {(States)state}");
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
        Debug.Log($"FSM: {m_ActiveState.State} ended. Request next state");
        p_CanActiveState = canActive;
        ChangeStateRequested.Invoke();
    }

    void Update()
    {
        m_ActiveState.MachineState.Update(Time.deltaTime);
    }
}
