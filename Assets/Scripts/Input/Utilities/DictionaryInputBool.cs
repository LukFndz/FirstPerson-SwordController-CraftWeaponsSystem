using System;

namespace FP.Input.Utilities
{
    [Serializable]
    public class DictionaryInputBool
    {
#if UNITY_EDITOR
        [InputActionSelector]
#endif
        public string ActionName;
        public bool Value;
    }
}