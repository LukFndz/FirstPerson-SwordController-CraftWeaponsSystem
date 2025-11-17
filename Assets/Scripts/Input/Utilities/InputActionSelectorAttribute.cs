#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FP.Input.Utilities
{
    public class InputActionSelectorAttribute : PropertyAttribute
    {
        private const string ASSET_PATH = "Assets/Misc/InputAction.inputactions";
        public InputActionAsset InputAsset
        {
            get
            {
                var guids = AssetDatabase.FindAssets("t:InputActionAsset");

                if (guids.Length == 0) { return null; }
            
                var asset = AssetDatabase.LoadAssetAtPath<InputActionAsset>(ASSET_PATH);
                return asset;
            }
            
        }
    }
}
#endif