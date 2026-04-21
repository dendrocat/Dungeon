using System;
using System.Collections.Generic;
using UnityEngine;
using TriInspector;

[CreateAssetMenu(fileName = "StatesConfig", menuName = "Config/StatesConfig")]
[HideMonoScript]
public class StatesConfig : ScriptableObject
{
    [DisableInPlayMode]
    [SerializeField] States m_StartState;

    [Serializable]
    public class StateEntry
    {
        [LabelWidth(50)]
        [SerializeField] States m_State;
        [SerializeReference] BaseState m_MachineState;

        public States State => m_State;
        public BaseState MachineState => m_MachineState;

        public StateEntry Clone()
        {
            var se = new StateEntry();
            se.m_State = m_State;
            se.m_MachineState = m_MachineState.Clone();
            return se;
        }
    }

    [DisableInPlayMode]
    [PropertySpace(15)]
    [TableList(Draggable = true, AlwaysExpanded = true)]
    [SerializeField] List<StateEntry> m_Mapping;

    [Serializable]
    class TransitionEntry
    {
        [LabelWidth(50)]
        [SerializeField] public States State;
        [SerializeField] public List<States> ExitStates;
    }
    [DisableInPlayMode]
    [PropertySpace(15)]
    [TableList(Draggable = true, AlwaysExpanded = true)]
    [SerializeField] List<TransitionEntry> m_Transitions;


    public StateEntry GetStartState()
    {
        return GetState(m_StartState);
    }

    public StateEntry GetState(States state)
    {
        return m_Mapping.Find(e => e.State == state).Clone();
    }

    public List<States> GetExitStates(States state)
    {
        return m_Transitions.Find(e => e.State == state)?.ExitStates ?? new List<States>();
    }
}
