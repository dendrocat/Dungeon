using TriInspector;
using UnityEngine;

[DeclareFoldoutGroup("set", Title = "Settings", Expanded = true)]
[DeclareFoldoutGroup("cmp", Title = "Components")]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour
{
    [Group("set")]
    [SerializeField, Slider(1, 10)] int m_Speed = 5;

    [Group("set")]
    [SerializeField, Slider(1, 10)] int m_JumpHeight = 1;
    Vector3 m_JumpForce;

    [Required, Group("cmp")]
    [SerializeField] GroundChecker m_GroundChecker;

    InputManager m_Input;


    private Rigidbody m_Rig;

    void Awake()
    {
        m_Rig = GetComponent<Rigidbody>();
        CalcJumpForce();
    }

    void Start()
    {
        m_Input = InputManager.Instance;
        m_Input.Jumped.AddListener(Jump);
    }

    void CalcJumpForce()
    {
        m_JumpForce = new Vector3(0, Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * m_JumpHeight), 0);
        m_JumpForce *= m_Rig.mass;
        Debug.Log($"JumpForce: {m_JumpForce}");
    }

    void Jump()
    {
        if (!m_GroundChecker.IsGrounded) return;
        m_Rig.AddForce(m_JumpForce, ForceMode.Impulse);
    }

    void Move()
    {
        var dir = new Vector3(m_Input.Move.x, 0, m_Input.Move.y).normalized;
        dir = transform.TransformDirection(dir);
        m_Rig.linearVelocity = new Vector3(dir.x * m_Speed, m_Rig.linearVelocity.y, dir.z * m_Speed);
    }

    void FixedUpdate()
    {
        Move();
    }
}
