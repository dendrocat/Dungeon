using UnityEngine;
using UnityEngine.Events;

public class StateExitor : StateMachineBehaviour
{
    public static event UnityAction<bool> StateExited;

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Debug.Log($"OnStateExit {GetInstanceID()}: {stateInfo.normalizedTime}");
        // Debug.Log($"StateExited == null {StateExited == null}");
        StateExited?.Invoke(stateInfo.normalizedTime >= 0.95f);
        // Debug.Log($"After StateExited: {stateInfo.normalizedTime}");
    }
}
