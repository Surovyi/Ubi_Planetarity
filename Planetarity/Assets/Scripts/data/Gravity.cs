using UnityEngine;

namespace game.data {
    /// <summary>
    /// Gravity information data container
    /// </summary>
    public class Gravity {
        /// <summary>
        /// Gravity direction (normalized)
        /// </summary>
        public Vector3 Direction = Vector3.zero;
        /// <summary>
        /// Gravity force
        /// </summary>
        public float Force = 0f;
    }
}