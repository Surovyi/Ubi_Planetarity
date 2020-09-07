using UnityEngine;

namespace game.extensions {
    /// <summary>
    /// Extensions to add a new handful functionality to already existing classes
    /// </summary>
    public static class VectorExtensions {
        /// <summary>
        /// Modifies vector to be in specified range on all 3 axis
        /// </summary>
        /// <param name="v">Target vector</param>
        /// <param name="minMax">Range for random</param>
        /// <returns></returns>
        public static Vector3 GetRandomSize(MinMaxFloat minMax) {
            return Vector3.one * Random.Range(minMax.Min, minMax.Max);
        }
    }
}