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

    void Awake()
    {
        m_ActiveState = m_Config.GetStartState();
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
        Debug.Log($"Change State to {state}");
        if (!Enum.IsDefined(typeof(States), state)) return false;
        if ((States)state == m_ActiveState.State) return true;

        m_ActiveState.MachineState.Exit();

        m_ActiveState = m_Config.GetState((States)state);

        m_ActiveState.MachineState.Enter(m_Enemy);
        m_ActiveState.MachineState.StateEnded += OnStateEnded;
        return true;
    }

    void OnStateEnded(bool canActive)
    {
        p_CanActiveState = canActive;
        ChangeStateRequested.Invoke();
    }

    void Update()
    {
        m_ActiveState.MachineState.Update(Time.deltaTime);
    }
}
