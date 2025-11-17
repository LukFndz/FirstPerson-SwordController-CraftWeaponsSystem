using FP.Input.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace FP.Input
{
    [CreateAssetMenu(fileName = "InputInitializer", menuName = "Shippin/Inputs/InputInitializer", order = 0)]
    public class InputInitializer : ScriptableObject
    {
        public List<DictionaryInputBool> InputInitialize = new();
    }
}