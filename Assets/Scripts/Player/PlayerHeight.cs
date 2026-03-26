using UnityEngine;
using TriInspector;

[DeclareFoldoutGroup("set", Title = "Settings", Expanded = true)]
[DeclareFoldoutGroup("cmp", Title = "Components")]
public class PlayerHeight : MonoBehaviour
{
    [Group("set")]
    [SerializeField] float m_CameraCrouchHeight = 0;
    float m_CameraStayHeight;

    [Group("set")]
    [SerializeField] float m_ColliderCrouchHeight = 1.5f;
    float m_ColliderStayHeight;

    [Group("set")]
    [SerializeField] float m_ColliderCrouchCenter = -0.25f;
    float m_ColliderStayCenter;

    [Group("set")]
    [SerializeField, Slider(0.5f, 10f)] float m_TransitionSpeed = 4f;
	const float c_StopTransition = 0.001f; 

    [Required, Group("cmp")]
    [SerializeField] Transform m_Cam;

    [Required, Group("cmp")]
    [SerializeField] CapsuleCollider m_Col;

    void Awake()
    {
        m_CameraStayHeight = m_Cam.localPosition.y;
        m_ColliderStayHeight = m_Col.height;
        m_ColliderStayCenter = m_Col.center.y;
    }

    void FixedUpdate()
    {
        float camY = IInput.Instance.IsCrouching ? m_CameraCrouchHeight : m_CameraStayHeight;

        if (Mathf.Abs(camY - m_Cam.localPosition.y) > c_StopTransition)
        {
            // DomainLogging.DomainDebug.Log($"{camY} {m_Cam.localPosition.y}", DomainLogging.DomainType.Player);
            Vector3 camPos = m_Cam.localPosition;
            camPos.y = camY;
            m_Cam.localPosition = Vector3.Lerp(
                    m_Cam.localPosition,
                    camPos,
                    m_TransitionSpeed * Time.fixedDeltaTime
            );
            if (Mathf.Abs(camY - m_Cam.localPosition.y) < c_StopTransition)
                m_Cam.localPosition = camPos;

            Vector3 colCen = m_Col.center;
            colCen.y = IInput.Instance.IsCrouching ? m_ColliderCrouchCenter : m_ColliderStayCenter;
            m_Col.center = colCen;
            m_Col.height = IInput.Instance.IsCrouching ? m_ColliderCrouchHeight : m_ColliderStayHeight;
        }


    }
}
