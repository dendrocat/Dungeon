using UnityEngine;

public class StateExitor : StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Debug.Log($"OnStateExit {GetInstanceID()}: {stateInfo.normalizedTime}");
        Debug.Log($"StateExited stateInfo {stateInfo.ToString()}");
		var callbackReciever = animator.GetComponentInParent<IAnimator>();
        callbackReciever?.OnStateExited(stateInfo.normalizedTime >= 0.95f);
        // Debug.Log($"After StateExited: {stateInfo.normalizedTime}");
    }
}
