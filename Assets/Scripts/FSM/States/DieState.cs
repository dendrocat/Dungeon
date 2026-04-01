using UnityEngine;
using TriInspector;

[System.Serializable]
public class DieState : BaseState
{
    [LabelWidth(100)]
    [Unit(UnitAttribute.Second)]
    [SerializeField, Min(5)] float m_DestroyTime = 5;

    Timer m_DestroyTimer;

    protected override void OnEnter()
    {
        GameObject.Destroy(p_Enemy.gameObject, m_DestroyTime);
    }
}
