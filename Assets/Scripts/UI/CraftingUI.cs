using UnityEngine;

namespace FP.Player.Crafting.UI
{
    public sealed class CraftingUI : MonoBehaviour
    {
        [SerializeField] private CraftingSystem _craftingSystem;
        [SerializeField] private WeaponRecipe _swordRecipe;
        [SerializeField] private WeaponRecipe _axeRecipe;

        public void OnCraftSwordButton()
        {
            _craftingSystem.Craft(_swordRecipe);
        }

        public void OnCraftAxeButton()
        {
            _craftingSystem.Craft(_axeRecipe);
        }
    }
}
