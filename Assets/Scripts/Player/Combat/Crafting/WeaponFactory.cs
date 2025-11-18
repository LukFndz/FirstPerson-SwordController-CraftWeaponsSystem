using UnityEngine;
using FP.Player.Combat.Weapon;

namespace FP.Player.Crafting
{
    public static class WeaponFactory
    {
        /// <summary>
        /// Creates a new runtime instance of a weapon.
        /// </summary>
        public static WeaponBase CreateWeapon(WeaponBase template)
        {
            return Object.Instantiate(template);
        }
    }
}
