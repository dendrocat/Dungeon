using TriInspector;
using UnityEngine;

[DeclareFoldoutGroup("set", Title = "Settings", Expanded = true)]
[DeclareFoldoutGroup("cmp", Title = "Components")]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour, IActivatable
{
    public bool IsActive => enabled;

    [Group("set")]
    [SerializeField, Slider(1, 10)] float m_BaseSpeed = 5;
    public float BaseSpeed => m_BaseSpeed;

    [Group("set")]
    [SerializeField, Slider(1, 10)] int m_JumpHeight = 1;
    Vector3 m_JumpForce;

    [Required, Group("cmp")]
    [SerializeField] GroundChecker m_GroundChecker;

    InputManager m_Input;


    private Rigidbody m_Rig;
    public float Speed {get; private set;} = 0;

    void Awake()
    {
        m_Rig = GetComponent<Rigidbody>();
		Speed = m_BaseSpeed;
        CalcJumpForce();
    }

    void Start()
    {
        m_Input = InputManager.Instance;
        m_Input.Jumped += Jump;
    }

    void CalcJumpForce()
    {
        m_JumpForce = new Vector3(0, Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * m_JumpHeight), 0);
        m_JumpForce *= m_Rig.mass;
        // Debug.Log($"JumpForce: {m_JumpForce}");
    }

    void Jump()
    {
        if (!m_GroundChecker.IsGrounded) return;
        m_Rig.AddForce(m_JumpForce, ForceMode.Impulse);
    }

    void Move()
    {
        var dir = new Vector3(m_Input.Move.x, 0, m_Input.Move.y).normalized;
        dir = transform.TransformDirection(dir).normalized;
        m_Rig.linearVelocity = new Vector3(dir.x * m_BaseSpeed, m_Rig.linearVelocity.y, dir.z * m_BaseSpeed);
    }

    void FixedUpdate()
    {
        Move();
    }

    public void Activate() => enabled = true;
    public void Deactivate() => enabled = false;
}
