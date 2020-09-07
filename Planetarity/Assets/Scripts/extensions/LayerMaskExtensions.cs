using UnityEngine;

namespace game.extensions {
    /// <summary>
    /// Extensions to add a new handful functionality to already existing classes
    /// </summary>
    public static class LayerMaskExtensions {
        /// <summary>
        /// Is specified layer checked in this LayerMask
        /// </summary>
        /// <param name="mask">Mask</param>
        /// <param name="layer">Layer to check</param>
        /// <returns>True if layer is checked in this mask</returns>
        public static bool Contains(this LayerMask mask, int layer) {
            return mask == (mask | (1 << layer));
        }
    }
}
