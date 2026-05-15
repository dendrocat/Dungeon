using UnityEngine;
using Unity.MLAgents.Sensors;

public struct AudioInput
{
    public Transform Transform;
    public float Radius;
    public int Mask;
}

public class AudioOutput
{
    public Vector3? AudioPosition = null;
    public float AudioLevel = 0;
}

public class AudioSensor : ISensor
{
    AudioInput m_Input;
    AudioOutput m_Output;

    public AudioOutput AudioOutput => m_Output;

    string m_Name;

    public AudioSensor(AudioInput input, string name)
    {
        m_Name = name;
        m_Input = input;
        m_Output = new();
    }

    public int Write(ObservationWriter writer)
    {
        writer[0] = m_Output.AudioLevel;
        return 1;
    }

    public byte[] GetCompressedObservation() => null;

    public void Reset() { }

    public void Update()
    {
        Collider[] hits = Physics.OverlapSphere(m_Input.Transform.position, m_Input.Radius, m_Input.Mask);
        float audioLevel;
		m_Output.AudioLevel = 0;
		m_Output.AudioPosition = null;
        foreach (var hit in hits)
        {
			// Debug.Log(hit.transform.parent.name);
            foreach (var emitter in hit.GetComponentsInChildren<IAudioEmitter>())
            {
                audioLevel = emitter.GetAudioLevel();
                var dist = Vector3.Distance(hit.transform.position, m_Input.Transform.position);
                audioLevel *= Mathf.Clamp01(1 - dist / m_Input.Radius);

                if (m_Output.AudioLevel > audioLevel) continue;
                m_Output.AudioLevel = audioLevel;
                m_Output.AudioPosition = hit.transform.position;
            }
        }
    }

    public string GetName() => m_Name;

    public CompressionSpec GetCompressionSpec() => CompressionSpec.Default();

    public ObservationSpec GetObservationSpec() => ObservationSpec.Vector(1);
}
