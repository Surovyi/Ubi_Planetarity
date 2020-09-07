using game.input;
using game.logic;
using game.views;

namespace game.data {
    /// <summary>
    /// Planet data container
    /// </summary>
    public class Planet : CelestialObject {
        /// <summary>
        /// Reference to planet view component
        /// </summary>
        public PlanetView PlanetView;
        /// <summary>
        /// Reference to move around component
        /// </summary>
        public MoveAround MoveAround;
        /// <summary>
        /// Reference to gun rotate component
        /// </summary>
        public GunRotate GunRotate;
        /// <summary>
        /// Reference to gun shoot component
        /// </summary>
        public GunShoot GunShoot;

        /// <summary>
        /// Is this planet under player control
        /// </summary>
        public bool UnderPlayerControl;
        /// <summary>
        /// Is this planet is still alive
        /// </summary>
        public bool Alive;
    }
}