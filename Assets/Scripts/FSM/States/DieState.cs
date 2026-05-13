using UnityEngine;
using TriInspector;

[System.Serializable]
public class DieState : BaseState
{
    [LabelWidth(100)]
    [Unit(UnitAttribute.Second)]
    [SerializeField, Min(5)] float m_DestroyTime = 5;

    protected override void OnEnter()
    {
		p_Enemy.Animator.Die();
        GameObject.Destroy(p_Enemy.gameObject, m_DestroyTime);
    }
}
