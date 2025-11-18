using FP.Player.Combat;
using UnityEngine;

namespace FP.Player.Crafting.UI
{
    // PLACEHOLDER UI SCRIPT FOR TEST
    public sealed class CraftingUI : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private CraftingSystem _craftingSystem;
        [SerializeField] private WeaponRecipe _swordRecipe;
        [SerializeField] private WeaponRecipe _axeRecipe;

        private bool _isOpen;

        public void OpenUI(WeaponController playerOpenController)
        {
            if (!_isOpen)
            {
                Cursor.visible = true;
                _craftingSystem.WeaponController = playerOpenController;
            }
            else
            {
                Cursor.visible = false;
            }

            _isOpen = !_isOpen;

            _panel.SetActive(_isOpen);
        }

        public void OnCraftSwordButton()
        {
            _craftingSystem.Craft(_swordRecipe);
        }

        public void OnCraftAxeButton()
        {
            _craftingSystem.Craft(_axeRecipe);
        }

        public void CloseUI()
        {
            if (!_isOpen) return;

            _isOpen = false;
            Cursor.visible = false;
            _panel.SetActive(false);
        }
    }
}
