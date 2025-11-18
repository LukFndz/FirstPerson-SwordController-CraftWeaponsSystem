using FP.Common;
using FP.Player.Crafting.UI;
using UnityEngine;

namespace FP.UI
{
    /// <summary>
    /// Class to manage UI components.
    /// </summary>
    public class UIManager : MonoBehaviourSingleton<UIManager>
    {
        [SerializeField] private AttackIndicatorUI _attackIndicatorUI;
        [SerializeField] private CraftingUI _craftingUI;

        public AttackIndicatorUI AttackIndicatorUI { get => _attackIndicatorUI; }
        public CraftingUI CraftingUI { get => _craftingUI; }
    }
}
