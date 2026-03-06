using UnityEngine;
using Unity.MLAgents.Sensors;
using System;

public struct AudioInput {
	public Transform Transform;
	public float Radius;
	public int Mask;
}

public class AudioOutput
{
    public float AudioLevel;
}

public class AudioSensor : ISensor
{
	AudioInput m_Input;
	AudioOutput m_Output;

	public AudioOutput Output => m_Output;

    string m_Name;


    public AudioSensor(AudioInput input, string name)
    {
        m_Name = name;
    }
    public int Write(ObservationWriter writer)
    {
        throw new NotImplementedException("Write in AudioSensor not implemented");
    }

    public byte[] GetCompressedObservation()
    {
        throw new NotImplementedException("GetCompressedObservation in AudioSensor not implemented");
    }

    public void Reset() { }

    public void Update()
    {
		Collider[] hits = Physics.OverlapSphere(m_Input.Transform.position, m_Input.Radius, m_Input.Mask);
		//TODO: write logic of hearing
    }

    public string GetName()
    {
        return m_Name;
    }

    public CompressionSpec GetCompressionSpec()
    {
        throw new NotImplementedException("GetCompressionSpec in AudioSensor not implemented");
    }
    public ObservationSpec GetObservationSpec()
    {
        throw new NotImplementedException("GetObservationSpec in AudioSensor not implemented");
    }
}
