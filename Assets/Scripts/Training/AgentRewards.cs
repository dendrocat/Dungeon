using UnityEngine;
using Slider = TriInspector.SliderAttribute;

[CreateAssetMenu(fileName = "AgentRewards", menuName = "Config/AgentRewards")]
public class AgentRewards : ScriptableObject
{
    [SerializeField, Slider(1, 300f)] float m_PlayerKill = 100f;
    public float PlayerKill => m_PlayerKill;

    [SerializeField, Slider(-150, 0)] float m_Die = -100f;
    public float Die => m_Die;

    [SerializeField, Slider(1, 400)] float m_Correct = 50f;
    public float Correct => m_Correct;

    [SerializeField, Slider(-500, 0)] float m_Incorrect = -200f;
    public float Incorrect => m_Incorrect;

    [SerializeField, Slider(-1000, 100)] float m_TimeEnd = -100f;
    public float TimeEnd => m_TimeEnd;
}
