namespace UnityEngine.InputSystem.Rebinding
{
    using TextUI = TMPro.TextMeshProUGUI;

    public class RebindAction : MonoBehaviour
    {
        [SerializeField] InputActionProperty m_Action;
        [SerializeField] InputBinding m_BindingMask;

        [SerializeField] string m_BindingId;

        [SerializeField] InputBinding.DisplayStringOptions m_DisplayStringOptions;

        [SerializeField] TextUI m_BindingText;

        public void StartRebind()
        {
            var action = m_Action.action;

            action.Disable();
            Debug.Log("Start Rebinding");
            action.PerformInteractiveRebinding()
				.WithCancelingThrough("<Keyboard>/escape")
				.OnComplete(op =>
					{
						Debug.Log("Complete");
						Debug.Log(m_Action.reference.asset.SaveBindingOverridesAsJson());
						action.Enable();
						op.Dispose();
						UpdateBindingDisplay();
					})
				.OnCancel(op =>
					{
						Debug.Log("Error");
						action.Enable();
						op.Dispose();
					})
				.Start();

        }

        void OnValidate()
        {
            UpdateBindingDisplay();
        }

        void UpdateBindingDisplay()
        {
            var action = m_Action.action;
            m_BindingText.text = action?.GetBindingDisplayString(m_DisplayStringOptions) ?? string.Empty;
        }
    }
}
