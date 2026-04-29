#if UNITY_EDITOR
using System.Linq;
using UnityEditor;

namespace UnityEngine.InputSystem.Rebinding
{

    [CustomEditor(typeof(RebindAction))]
    public class RebindActionEditor : UnityEditor.Editor
    {
        static class Styles
        {
            public static GUIStyle boldLabel = new GUIStyle("BoldLabel");
        }

        SerializedProperty m_ActionProperty;
        SerializedProperty m_BindingIndexProperty;
        SerializedProperty m_DisplayStringOptionsProperty;
        SerializedProperty m_BindingTextProperty;

        GUIContent m_BindingLabel = new GUIContent("Binding");
        GUIContent m_DisplayOptionsLabel = new GUIContent("Display Options");
        GUIContent m_UILabel = new GUIContent("UI");

        GUIContent[] m_BindingOptions;
        int[] m_BindingOptionValues;
        int m_SelectedBindingOption;

        void OnEnable()
        {
            m_ActionProperty = serializedObject.FindProperty("m_Action");
            m_BindingIndexProperty = serializedObject.FindProperty("m_BindingIndex");
            m_DisplayStringOptionsProperty = serializedObject.FindProperty("m_DisplayStringOptions");
            m_BindingTextProperty = serializedObject.FindProperty("m_BindingText");

            RefreshBindingOptions();
        }

        void RefreshBindingOptions()
        {
            var actionProperty = (InputActionReference)m_ActionProperty.boxedValue;
            var action = actionProperty?.action;
            if (action == null)
            {
                m_BindingOptions = new GUIContent[0];
                m_BindingOptionValues = new int[0];
                m_SelectedBindingOption = -1;
                return;
            }
            var bindings = action.bindings;
            int cntNotComposite = action.bindings.Count(b => !b.isComposite);

            m_BindingOptions = new GUIContent[cntNotComposite];
            m_BindingOptionValues = new int[cntNotComposite];
            m_SelectedBindingOption = -1;

            var currentBindingId = m_BindingIndexProperty.intValue;
            for (int i = 0, k = 0; i < bindings.Count; ++i)
            {
                if (bindings[i].isComposite) continue;

                var binding = bindings[i];
                var bindingId = i;
                var haveBindingGroups = !string.IsNullOrEmpty(binding.groups);

                var displayOptions =
                    InputBinding.DisplayStringOptions.DontUseShortDisplayNames | InputBinding.DisplayStringOptions.IgnoreBindingOverrides;
                if (!haveBindingGroups)
                    displayOptions |= InputBinding.DisplayStringOptions.DontOmitDevice;

                var displayString = action.GetBindingDisplayString(i, displayOptions);

                // If binding is part of a composite, include the part name.
                if (binding.isPartOfComposite)
                    displayString = $"{ObjectNames.NicifyVariableName(binding.name)}: {displayString}";

                displayString = displayString.Replace('/', '\\');

                // If the binding is part of control schemes, mention them.
                if (haveBindingGroups)
                {
                    var asset = action.actionMap?.asset;
                    if (asset != null)
                    {
                        var controlSchemes = string.Join(", ",
                            binding.groups.Split(InputBinding.Separator)
                                .Select(x => asset.controlSchemes.FirstOrDefault(c => c.bindingGroup == x).name));

                        displayString = $"{displayString} ({controlSchemes})";
                    }
                }

                m_BindingOptions[k] = new GUIContent(displayString);
                m_BindingOptionValues[k] = bindingId;

                if (currentBindingId == bindingId)
                    m_SelectedBindingOption = k;
                ++k;
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            // Binding section.
            EditorGUILayout.LabelField(m_BindingLabel, Styles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(m_ActionProperty);
                if (EditorGUI.EndChangeCheck()) RefreshBindingOptions();

                var newSelectedBinding = EditorGUILayout.Popup(m_BindingLabel, m_SelectedBindingOption, m_BindingOptions);
                if (newSelectedBinding == -1 && m_BindingOptions.Length > 0) newSelectedBinding = 0;

                // Debug.Log($"{m_SelectedBindingOption} {newSelectedBinding}");
                if (newSelectedBinding != m_SelectedBindingOption)
                {
                    var bindingId = m_BindingOptionValues[newSelectedBinding];
                    m_BindingIndexProperty.intValue = bindingId;
                    m_SelectedBindingOption = newSelectedBinding;
                }
                // Debug.Log(m_BindingIndexProperty.intValue);

                var optionsOld = (InputBinding.DisplayStringOptions)m_DisplayStringOptionsProperty.intValue;
                var optionsNew = (InputBinding.DisplayStringOptions)EditorGUILayout.EnumFlagsField(m_DisplayOptionsLabel, optionsOld);
                if (optionsOld != optionsNew)
                    m_DisplayStringOptionsProperty.intValue = (int)optionsNew;
            }

            // UI section.
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(m_UILabel, Styles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUILayout.PropertyField(m_BindingTextProperty);
            }

            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
