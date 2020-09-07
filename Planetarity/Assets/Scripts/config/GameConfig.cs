using System;
using game.extensions;

namespace game.config {
    /// <summary>
    /// Configuration asset for overall game variables
    /// </summary>
    [Serializable]
    public class GameConfig {
        /// <summary>
        /// Draws orbits around planets
        /// </summary>
        public bool DrawOrbits;

        /// <summary>
        /// Draws gravity field around planets
        /// NOTE: Implemented only visually
        /// </summary>
        public bool DrawGravityField;

        /// <summary>
        /// Allows planet to move in reverse direction
        /// </summary>
        public bool AllowReversePlanetMove;

        /// <summary>
        /// Clamps max gravity applied to rockets to avoid very powerful fly direction change
        /// </summary>
        public bool ClampMaxGravity;

        /// <summary>
        /// AI players count
        /// </summary>
        public MinMaxInt AiPlayersRange;

        /// <summary>
        /// Radius from the Sun in which all planets will be spawned
        /// </summary>
        public MinMaxInt PlanetSpawnRadiusRange;

        /// <summary>
        /// Random planet color
        /// </summary>
        public MinMaxInt PlanetHueRange;

        /// <summary>
        /// Random planet mass
        /// </summary>
        public MinMaxFloat PlanetMassRange;

        /// <summary>
        /// Random planet size
        /// </summary>
        public MinMaxFloat PlanetSizeRange;

        /// <summary>
        /// Random planet gravity field
        /// NOTE: Feature not implemented
        /// </summary>
        public MinMaxFloat PlanetGravityRange;

        /// <summary>
        /// Random planet orbit speed
        /// </summary>
        public MinMaxFloat PlanetMoveSpeed;

        /// <summary>
        /// Bounds for camera orthographic size
        /// </summary>
        public MinMaxFloat CameraSizeRange;

        /// <summary>
        /// Should planet be spawned on the same orbit point
        /// </summary>
        public bool SpawnPlanetsOnTheSameLine;

        /// <summary>
        /// Mass of the Sun
        /// </summary>
        public float SunMass;

        /// <summary>
        /// Start planet HP value
        /// </summary>
        public int PlanetHp;

        /// <summary>
        /// Minimal distance between two spawned planets
        /// </summary>
        public float SpawnPlanetMinDistance;

        /// <summary>
        /// Gravitation constance from the Newtonian law of gravitation
        /// Configuring this value gives easy access to gravity control
        /// </summary>
        public float UniverseGravitationalConstant;

        /// <summary>
        /// Clamps max gravity to specified value applied to rockets. Helps avoid very powerful fly direction change
        /// </summary>
        public float MaxGravityForce;
    }
}