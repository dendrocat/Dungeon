using UnityEngine;
using TriInspector;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/PlayerConfig")]
public class PlayerConfig : PersonConfig<PlayerHealthConfig>
{
	[Group("sp"), Unit(UnitAttribute.Meter)]
    [SerializeField, Slider(1, 10)] int m_JumpHeight = 1;
	public int JumpHeight => m_JumpHeight;
}
