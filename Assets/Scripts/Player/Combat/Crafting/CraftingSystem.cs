using UnityEngine;
using FP.Player.Resources;
using FP.Player.Combat;
using FP.Player.Crafting;

namespace FP.Player.Crafting
{
    public sealed class CraftingSystem : MonoBehaviour
    {
        [SerializeField] private PlayerResourceCounter _resourceCounter;
        [SerializeField] private WeaponController _weaponController;

        public void Craft(WeaponRecipe recipe)
        {
            if (!_resourceCounter.HasResources(recipe.WoodCost, recipe.IronCost))
                return;

            _resourceCounter.Consume(recipe.WoodCost, recipe.IronCost);

            _weaponController.EquipWeapon(
                recipe.WeaponPrefab,
                recipe.WeaponData
            );
        }
    }
}
