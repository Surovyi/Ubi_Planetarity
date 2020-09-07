using System;

namespace game.config {
    /// <summary>
    /// Configuration asset for rocket variables
    /// </summary>
    [Serializable]
    public class RocketConfig {
        /// <summary>
        /// Type of rocket
        /// NOTE: Supports more then one different rocket types, but physically and graphically types are not implemented
        /// </summary>
        public int RocketType;

        /// <summary>
        /// Initial acceleration added to rocket
        /// </summary>
        public float InitialAccelerationMultiplier;

        /// <summary>
        /// Rocket mass. Used in gravity calculations.
        /// </summary>
        public float Mass;

        /// <summary>
        /// Cooldown applied after shooting rocket
        /// </summary>
        public float Cooldown;

        /// <summary>
        /// Lifetime for rocket
        /// </summary>
        public float Lifetime;

        /// <summary>
        /// Damage applied if rocket hits damageable object
        /// </summary>
        public int Damage;
    }
}