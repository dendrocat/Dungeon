using System;

public class Timer
{
    public event Action TimerEnded;

    float m_Timer = 0, m_MaxTimer;
    public Timer(float seconds) { m_MaxTimer = seconds; }

    public void Update(float dt)
    {
        m_Timer += dt;
        if (m_Timer >= m_MaxTimer) TimerEnded?.Invoke();
    }

    public void Reset() { m_Timer = 0; }
}
