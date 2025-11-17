#if UNITY_EDITOR && ENABLE_INPUT_SYSTEM
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace FP.Input.Utilities
{
    [CustomPropertyDrawer(typeof(InputActionSelectorAttribute))]
    public class InputActionSelectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DrawDropdown(position, property, label);
        }
        
        public void DrawDropdown(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = (InputActionSelectorAttribute)attribute;
            if(attr == null) { return; }
            
            var asset = attr.InputAsset;
            
            if (asset is null)
            {
                EditorGUI.HelpBox(position, $"Input actions are not defined.", MessageType.Error);
                return;
            }

            var allActions = asset.ToList();
            var actionNames = allActions.Select(a => a.name).ToArray();

            if (actionNames.Length == 0)
            {
                EditorGUI.HelpBox(position,  $"Input actions are not defined.", MessageType.Error);
                return;
            }
            
            var index = System.Array.IndexOf(actionNames, property.stringValue);
            if (index < 0) { index = 0; }
            
            var newIndex = EditorGUI.Popup(position, label.text, index, actionNames);
            property.stringValue = actionNames[newIndex];
        }
    }
}
#endif