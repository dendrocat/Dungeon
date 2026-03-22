using UnityEngine;
using TriInspector;

[CreateAssetMenu(fileName = "ValidatorConfig", menuName = "Config/AgentValidatorConfig")]
[DeclareBoxGroup("det", Title = "Detection")]
[DeclareBoxGroup("rew", Title = "Rewards")]
public class AgentValidatorConfig : ScriptableObject
{
    [System.Serializable]
    public class DetectionConfig
    {
        [SerializeField, Slider(5f, 20f)] float m_RayLength = 10f;
        public float RayLength => m_RayLength;

        [SerializeField, Slider(0f, 1f)] float m_AudioLevel = 0.5f;
        public float AudioLevel => m_AudioLevel;
    }
    [Group("det"), InlineProperty, HideLabel]
    [SerializeField] DetectionConfig m_Detection;
    public DetectionConfig Detection => m_Detection;


    [System.Serializable]
    public class AgentRewards
    {
        [SerializeField, Slider(1, 150f)] float m_PlayerKill = 100f;
        public float PlayerKill => m_PlayerKill;

        [SerializeField, Slider(-150, 0)] float m_Die = -100f;
        public float Die => m_Die;

        [SerializeField, Slider(-200, 0)] float m_GroupDie = -150f;
        public float GroupDie => m_Die;

        [SerializeField, Slider(1, 100)] float m_Correct = 50f;
        public float Correct => m_Correct;

        [SerializeField, Slider(-300, 0)] float m_Incorrect = -200f;
        public float Incorrect => m_Incorrect;
    }
    [Group("rew"), InlineProperty, HideLabel]
	[SerializeField] AgentRewards m_Rewards;
	public AgentRewards Rewards => m_Rewards; 
}
