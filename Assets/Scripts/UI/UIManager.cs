using FP.Common;
using UnityEngine;

namespace FP.UI
{
    /// <summary>
    /// Class to manage UI components.
    /// </summary>
    public class UIManager : MonoBehaviourSingleton<UIManager>
    {
        [SerializeField] private AttackIndicatorUI _attackIndicatorUI;

        public AttackIndicatorUI AttackIndicatorUI { get => _attackIndicatorUI; }
    }
}
