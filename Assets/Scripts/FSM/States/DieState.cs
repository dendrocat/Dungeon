[System.Serializable]
public class DieState : BaseState
{
    protected override void OnEnter()
    {
        p_Enemy.NavAgent.ResetPath();
        p_Enemy.NavAgent.isStopped = true;
    }
}
