using System;

/// <summary>
/// A simple reusable timer that invokes callback when time expires.
/// </summary>
public class Timer
{
    /// <summary>
    /// Event invoked when timer expires.
    /// </summary>
    public event Action TimerEnded;

    /// <summary>Current elapsed time (in seconds).</summary>
    float m_Timer = 0;

    /// <summary>Total duration of the timer (in seconds).</summary>
    float m_MaxTimer;

    /// <summary>
    /// Initializes a new <c>Timer</c> with specified duration
    /// </summary>
    /// <param name="seconds">Timer duration in seconds (must be > 0).</param>
    public Timer(float seconds)
    {
        if (seconds <= 0) throw new ArgumentException("Seconds must be > 0");
        m_MaxTimer = seconds;
    }

    /// <summary>
    /// Updates the timer with the given delta time.
    /// Call this from MonoBehaviour's Update() or FixedUpdate().
    /// </summary>
    /// <param name="dt">Delta time since last frame (Time.deltaTime or Time.fixedDeltaTime).</param>
    public void Update(float dt)
    {
        m_Timer += dt;
        if (m_Timer >= m_MaxTimer) TimerEnded?.Invoke();
    }

    /// <summary>
    /// Resets the timer to zero, keeping the original duration.
    /// </summary>
    public void Reset() { m_Timer = 0; }

    /// <summary>
    /// Resets the timer and sets a new duration.
    /// </summary>
    /// <param name="seconds">New timer duration in seconds.</param>
    public void Reset(float seconds)
    {
        if (seconds <= 0) throw new ArgumentException("Seconds must be > 0");
        m_Timer = 0; m_MaxTimer = seconds;
    }
}
