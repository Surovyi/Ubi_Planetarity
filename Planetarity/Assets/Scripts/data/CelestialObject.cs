using UnityEngine;

namespace game.data {
    /// <summary>
    /// Base class for all celestial objects taking action in space
    /// </summary>
    public class CelestialObject {
        /// <summary>
        /// Main object transform component
        /// </summary>
        public Transform Transform;
        /// <summary>
        /// Specified mass for this celestial object
        /// </summary>
        public float Mass;
    }

}
