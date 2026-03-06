using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using System;

[RequireComponent(typeof(StateMachine))]
public class EnemyAgent : Agent
{
	StateMachine fsm;
	EnemyManager manager;

	public override void Initialize()
	{
		manager = GetComponent<EnemyManager>();

		fsm = GetComponent<StateMachine>();
		fsm.ChangeStateRequested.AddListener(RequestAction);
	}

	void Start() {
		InputManager.Instance.WeaponNumed.AddListener(OnNum);
	}

	void OnNum(int state) {
		this.state = state;
		Debug.Log($"On Numed");
	}

	int state = 0;

    public override void Heuristic(in ActionBuffers actionsOut)
    {
		Debug.Log("Heuristic");
		var d = actionsOut.DiscreteActions;
		d[0] = state;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
		fsm.ChangeState(state);
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
		for (int i = 0; i < Enum.GetValues(typeof(States)).Length; ++i)
			actionMask.SetActionEnabled(0, i, false);

		foreach (var st in fsm.GetExitStates()) 
			actionMask.SetActionEnabled(0, st, true);
    }
}
