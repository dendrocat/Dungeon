using UnityEngine;

public class PlayerWalkAudio : MonoBehaviour
{
    [SerializeField] GroundChecker m_Checker;
    [SerializeField] InputManager m_Input;
    [SerializeField] PlayerMover m_Mover;
    [SerializeField] PlayerAudioEmitter m_Emitter;

	[Space(15)]
    [SerializeField] AudioSource m_Step;
    [SerializeField] AudioClip m_RunClip;
    [SerializeField] AudioClip m_WalkClip;
    [SerializeField] AudioClip m_CrouchClip;

    void SetStep(AudioClip clip)
    {
        if (m_Step.clip == clip) return;
        m_Step.clip = clip;
    }

    void FixedUpdate()
    {
        if (m_Step.volume != m_Emitter.Volume)
            m_Step.volume = m_Emitter.Volume;

        if (!m_Checker.IsGrounded) m_Step.Pause();
        else if (m_Mover.Speed == 0) m_Step.Pause();
        else if (m_Mover.Speed > 0)
        {
            if (m_Input.IsRunning)
                SetStep(m_RunClip);
            else if (m_Input.IsCrouching)
                SetStep(m_CrouchClip);
            else SetStep(m_WalkClip);

            if (!m_Step.isPlaying)
                m_Step.Play();
        }
    }
}
