using UnityEngine;
using UnityEngine.Serialization;
using Unity.MLAgents.Sensors;
using TriInspector;

public class AudioSensorComponent : SensorComponent
{
    [SerializeField] string m_SensorName = "AudioSensor";

    [SerializeField, Slider(5, 100)] float m_AudioRadius = 20;
	[SerializeField] LayerMask m_AudioMask;

    [Header("Debug Colors")]
    [FormerlySerializedAs("ListenColor")]
    [SerializeField] Color m_HearColor = Color.red;

    [FormerlySerializedAs("MissColor")]
    [SerializeField] Color m_MissColor = Color.white;

    AudioSensor m_Sensor;

	AudioInput GetAudioInput() {
		var input =  new AudioInput();

		input.Transform = transform;
		input.Radius = m_AudioRadius;
		input.Mask = m_AudioMask.value;

		return input;
	}

    public override ISensor[] CreateSensors()
    {
        m_Sensor = new AudioSensor(GetAudioInput(), m_SensorName);
        return new[] { m_Sensor };
    }

    void OnDrawGizmosSelected()
    {
        if (m_Sensor?.Output?.AudioLevel == null) return;
        float lerpT = m_Sensor.Output.AudioLevel * m_Sensor.Output.AudioLevel;

        Gizmos.color = Color.Lerp(m_HearColor, m_MissColor, lerpT);
        Gizmos.DrawWireSphere(transform.position, m_AudioRadius);
    }
}

