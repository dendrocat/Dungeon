using TriInspector;
using UnityEngine;

[DeclareFoldoutGroup("set", Title = "Settings", Expanded = true)]
[DeclareFoldoutGroup("cmp", Title = "Components")]
public class MouseLook : MonoBehaviour, IActivatable
{
    public bool IsActive => enabled;

    [Slider(10, 100), Group("set")]
    [SerializeField] float m_Sensivity = 50;

    [Slider(70, 90), Group("set")]
    [SerializeField] int m_MaxAngle = 80;

    [Required, Group("cmp")]
    [SerializeField] Transform m_Cam;

    IInput m_Input;

    float xRot = 0;

    void Start()
    {
        m_Input = IInput.Instance;
    }

    void FixedUpdate()
    {
        var dt = m_Input.MouseDelta * m_Sensivity * Time.fixedDeltaTime;

        xRot -= dt.y;
        xRot = Mathf.Clamp(xRot, -m_MaxAngle, m_MaxAngle);
        m_Cam.localRotation = Quaternion.Euler(xRot, 0, 0);

        transform.Rotate(Vector3.up * dt.x);
    }

    public void Activate() => enabled = true;
    public void Deactivate() => enabled = false;
}
