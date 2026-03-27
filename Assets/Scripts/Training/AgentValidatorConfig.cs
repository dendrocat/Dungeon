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
        [SerializeField, Slider(0.1f, 1f)] float m_RayLengthScale = 0.5f;
        public float RayLengthScale => m_RayLengthScale;

        [SerializeField, Slider(0f, 1f)] float m_AudioLevel = 0.5f;
        public float AudioLevel => m_AudioLevel;
    }
    [Group("det"), InlineProperty, HideLabel]
    [SerializeField] DetectionConfig m_Detection;
    public DetectionConfig Detection => m_Detection;


    [System.Serializable]
    public class AgentRewards
    {
        [SerializeField, Slider(1, 300f)] float m_PlayerKill = 100f;
        public float PlayerKill => m_PlayerKill;

        [SerializeField, Slider(-150, 0)] float m_Die = -100f;
        public float Die => m_Die;

        [SerializeField, Slider(-200, 0)] float m_GroupDie = -150f;
        public float GroupDie => m_GroupDie;

        [SerializeField, Slider(1, 400)] float m_Correct = 50f;
        public float Correct => m_Correct;

        [SerializeField, Slider(-500, 0)] float m_Incorrect = -200f;
        public float Incorrect => m_Incorrect;
    }
    [Group("rew"), InlineProperty, HideLabel]
    [SerializeField] AgentRewards m_Rewards;
    public AgentRewards Rewards => m_Rewards;
}
