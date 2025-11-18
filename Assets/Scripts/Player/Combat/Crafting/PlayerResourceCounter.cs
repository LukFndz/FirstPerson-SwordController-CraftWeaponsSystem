using UnityEngine;

namespace FP.Player.Resources
{
    public sealed class PlayerResourceCounter : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private int _wood;
        [SerializeField] private int _iron;
        #endregion

        #region Properties
        public int Wood => _wood;
        public int Iron => _iron;
        #endregion

        #region Public Functions
        public bool HasResources(int wood, int iron)
        {
            return _wood >= wood && _iron >= iron;
        }

        public void Consume(int wood, int iron)
        {
            _wood -= wood;
            _iron -= iron;
        }

        public void Add(int wood, int iron)
        {
            _wood += wood;
            _iron += iron;
        }
        #endregion
    }
}
