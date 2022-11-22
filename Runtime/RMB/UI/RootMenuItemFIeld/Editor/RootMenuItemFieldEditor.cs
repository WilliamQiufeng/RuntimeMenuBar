using RMB.UI.RootMenuItemFIeld;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace RMB.UI.RootMenuItemField.Editor
{
    [CustomEditor(typeof(RootMenuItemFieldComponent))]
    public class RootMenuItemFieldEditor : UnityEditor.Editor
    {
        [SerializeField] VisualTreeAsset m_Uxml;

        public override VisualElement CreateInspectorGUI()
        {
            var parent = new VisualElement();

            m_Uxml?.CloneTree(parent);

            return parent;
        }
    }
}