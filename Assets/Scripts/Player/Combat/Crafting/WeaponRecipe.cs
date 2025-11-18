using UnityEngine;
using FP.Player.Combat.Weapon;

namespace FP.Player.Crafting
{
    [CreateAssetMenu(menuName = "Crafting/Weapon Recipe")]
    public sealed class WeaponRecipe : ScriptableObject
    {
        [SerializeField] private WeaponBase _weaponData;
        [SerializeField] private GameObject _weaponPrefab;
        [SerializeField] private int _woodCost;
        [SerializeField] private int _ironCost;

        public WeaponBase WeaponData => _weaponData;
        public GameObject WeaponPrefab => _weaponPrefab;
        public int WoodCost => _woodCost;
        public int IronCost => _ironCost;
    }
}
