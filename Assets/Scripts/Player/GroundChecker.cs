using TriInspector;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool IsGrounded { get; private set; }

    [SerializeField, Slider(0.1f, 2)] float m_CheckDistanse = 0.4f;
    [SerializeField, Slider(0.01f, 0.2f)] float m_Skin = 0.05f;
    [SerializeField] LayerMask m_Layer;


    void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * (m_CheckDistanse + m_Skin));
    }

    void FixedUpdate()
    {
        IsGrounded = Physics.Raycast(transform.position, Vector3.down, m_CheckDistanse + m_Skin, m_Layer);
    }
}
