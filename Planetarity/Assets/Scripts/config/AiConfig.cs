using System;
using game.extensions;

namespace game.config {
    /// <summary>
    /// Configuration asset for AI variables
    /// </summary>
    [Serializable]
    public class AiConfig {
        /// <summary>
        /// Time to be added on top of the rocket cooldown
        /// </summary>
        public MinMaxFloat AdditionalShootCooldownTime;

        /// <summary>
        /// Delay before making first shoot
        /// </summary>
        public MinMaxFloat StartShootDelayRange;

        /// <summary>
        /// Angle which will be added before shooting
        /// </summary>
        public MinMaxFloat ShootAngleRandomCorrection;
    }
}