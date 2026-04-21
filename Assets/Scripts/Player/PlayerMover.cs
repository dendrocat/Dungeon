using UnityEngine;
using TriInspector;

[DeclareFoldoutGroup("cmp", Title = "Components")]
[RequireComponent(typeof(Rigidbody), typeof(Player))]
public class PlayerMover : MonoBehaviour
{
    float m_BaseSpeed = 5;
    public float BaseSpeed => m_BaseSpeed;

    float m_Multiplier;
    int m_JumpHeight;

    Vector3 m_JumpForce;

    [Required, Group("cmp")]
    [SerializeField] GroundChecker m_GroundChecker;

    IInput m_Input;


    private Rigidbody m_Rig;
    public float Speed { get; private set; } = 0;

    public void Init(PlayerConfig config)
    {
        m_BaseSpeed = config.Speed.BaseSpeed;
        m_Multiplier = config.Speed.Multiplier;
        m_JumpHeight = config.JumpHeight;

        m_Rig = GetComponent<Rigidbody>();
        m_Input = GetComponent<Player>().Input;
        m_Input.Jumped += Jump;
    }

    void CalcJumpForce()
    {
        m_JumpForce = new Vector3(0, Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * m_JumpHeight), 0);
        m_JumpForce *= m_Rig.mass;
        // DomainLogging.DomainDebug.Log($"JumpForce: {m_JumpForce}", DomainLogging.DomainType.Player);
    }

    void Jump()
    {
        if (!m_GroundChecker.IsGrounded) return;
        m_Rig.AddForce(m_JumpForce, ForceMode.Impulse);
    }

    float CalcSpeed()
    {
        if (m_Input.IsRunning) return m_BaseSpeed * m_Multiplier;
        if (m_Input.IsCrouching) return m_BaseSpeed / m_Multiplier;
        return m_BaseSpeed;
    }

    void Move()
    {
        Speed = CalcSpeed();
        var dir = new Vector3(m_Input.Move.x, 0, m_Input.Move.y).normalized;
        dir = transform.TransformDirection(dir).normalized;
        m_Rig.linearVelocity = new Vector3(dir.x * Speed, m_Rig.linearVelocity.y, dir.z * Speed);
    }

    void FixedUpdate()
    {
        Move();
    }
}
