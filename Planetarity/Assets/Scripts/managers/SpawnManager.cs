using System.Collections.Generic;
using game.config;
using game.data;
using game.extensions;
using game.logic;
using game.logic.draw;
using game.views;
using UnityEngine;
using UnityEngine.Assertions;

namespace game.managers {
    /// <summary>
    /// Managers which spawns objects on the scene
    /// </summary>
    public class SpawnManager : MonoBehaviour {
        /// <summary>
        /// Reference to the Sun in the middle of the star system
        /// </summary>
        public Transform Sun;
        /// <summary>
        /// Reference to planet prefab
        /// </summary>
        public PlanetView PlanetPrefab;
        /// <summary>
        /// Reference to spawn destination
        /// </summary>
        public Transform PlanetsParent;
        /// <summary>
        /// Color for gravity fields
        /// </summary>
        public Color GravityColor = Color.white;

        private GameConfig _gameConfig;
        private int _planetsCount;
        private int _aiCount;

        private void Awake() {
            // Ensure that all references specified in Inspector
            Assert.IsNotNull(Sun, "SpawnManager.Awake => Sun != null");
            Assert.IsNotNull(PlanetPrefab, "SpawnManager.Awake => PlanetPrefab != null");
            Assert.IsNotNull(PlanetsParent, "SpawnManager.Awake => PlanetsParent != null");
        }

        /// <summary>
        /// Spawns planets using configuration data
        /// </summary>
        /// <param name="gameConfig">Configuration asset</param>
        /// <returns>List of spawned planets</returns>
        public List<Planet> SpawnPlanets(GameConfig gameConfig) {
            _gameConfig = gameConfig;
            _aiCount = Random.Range(_gameConfig.AiPlayersRange.Min, _gameConfig.AiPlayersRange.Max);
            _planetsCount = _aiCount + 1;

            Debug.Log($"SpawnManager.Awake => _aiCount: {_aiCount}");
            Debug.Log($"SpawnManager.Awake => _planetsCount: {_planetsCount}");

            return SpawnPlanets(_planetsCount);
        }


        private List<Planet> SpawnPlanets(int count) {
            List<Planet> planets = new List<Planet>();

            // Storing last spawn planet distance from the Sun
            float lastSpawnRadiusValue = _gameConfig.PlanetSpawnRadiusRange.Min;
            for (int i = 0; i < count; i++) {
                int planetNumber = i + 1;

                // Calculating new spawn location
                Vector3 position = CalculateSpawnPosition(planetNumber, ref lastSpawnRadiusValue);
                PlanetView planetView = SpawnNewPlanet(position);
                MoveAround moveAround = InitPlanetMovement(planetView);

                // Draw planet orbit if necessary
                if (_gameConfig.DrawOrbits) {
                    DrawOrbit(planetView);
                }

                // Draw planet gravity field if necessary
                if (_gameConfig.DrawGravityField) {
                    DrawGravityField(planetView);
                }

                // Generate mass and assign it to planet
                float mass = Random.Range(_gameConfig.PlanetMassRange.Min, _gameConfig.PlanetMassRange.Max);
                Planet planet = new Planet {
                    PlanetView = planetView,
                    MoveAround = moveAround,
                    Mass = mass,
                    Alive = true
                };

                // Init HP for this planet
                planetView.HpController.Init(_gameConfig.PlanetHp, planet);

                // Store this planet
                planets.Add(planet);
            }

            return planets;
        }

        /// <summary>
        /// Calculates new spawn position for planet
        /// </summary>
        /// <param name="planetNumber">Planet number</param>
        /// <param name="lastSpawnRadiusValue">Last spawned planet distance from the Sun</param>
        /// <returns></returns>
        private Vector3 CalculateSpawnPosition(int planetNumber, ref float lastSpawnRadiusValue) {
            Vector3 position = Vector3.one;

            // Calculate new spawn distance from the Sun
            float spawnRange = (_gameConfig.PlanetSpawnRadiusRange.Max - _gameConfig.PlanetSpawnRadiusRange.Min) /
                               (float) _planetsCount;
            float maxSpawnRadiusValue = _gameConfig.PlanetSpawnRadiusRange.Min + planetNumber * spawnRange;
            float spawnRadius = Random.Range(lastSpawnRadiusValue, maxSpawnRadiusValue);

            Debug.Log(
                $"SpawnManager.SpawnPlanets => [{planetNumber}][{lastSpawnRadiusValue:F},{maxSpawnRadiusValue:F}]: {spawnRadius}");

            position = Vector3.forward * spawnRadius;
            lastSpawnRadiusValue = spawnRadius + _gameConfig.SpawnPlanetMinDistance;

            // If we spawning not on the same line - get random position on planet orbit
            if (_gameConfig.SpawnPlanetsOnTheSameLine == false) {
                position = MoveAround.GetOrbitPosition(Random.Range(0f, 10f), (position - Sun.position).magnitude);
            }

            return position;
        }

        /// <summary>
        /// Spawns new planet on location
        /// </summary>
        /// <param name="position">Location of a new planet</param>
        /// <returns>View reference on a new planet</returns>
        private PlanetView SpawnNewPlanet(Vector3 position) {
            PlanetView planet = Instantiate(PlanetPrefab, position, Quaternion.Euler(45f, -45, 0f), PlanetsParent);

            Vector3 planetSize = VectorExtensions.GetRandomSize(_gameConfig.PlanetSizeRange);

            // Set all needed references
            planet.GraphicsRenderer.SetRandomColor(_gameConfig.PlanetHueRange.Min, _gameConfig.PlanetHueRange.Max);
            planet.GraphicsTransform.localScale = planetSize;
            planet.GunView.GunRenderer.color = planet.GraphicsRenderer.color;
            planet.GunView.GunTransform.localScale = planetSize;
            planet.Collider.radius = planetSize.x * 1.2f;

            return planet;
        }

        /// <summary>
        /// Draw orbit around specified planet
        /// </summary>
        /// <param name="planet">Planet</param>
        /// <returns>Orbit</returns>
        private DrawOrbit DrawOrbit(PlanetView planet) {
            DrawOrbit planetOrbit = planet.gameObject.AddComponent<DrawOrbit>();
            planetOrbit.RelativeTo = Sun;
            planetOrbit.LineRenderer = planet.OrbitLine;
            planetOrbit.OrbitColor = planet.GraphicsRenderer.color;

            return planetOrbit;
        }

        /// <summary>
        /// Draw gravity field around specified planet
        /// </summary>
        /// <param name="planet">Planet</param>
        /// <returns>Gravity field</returns>
        private DrawGravityField DrawGravityField(PlanetView planet) {
            float gravityRange = Random.Range(_gameConfig.PlanetGravityRange.Min, _gameConfig.PlanetGravityRange.Max);

            DrawGravityField planetGravity = planet.gameObject.AddComponent<DrawGravityField>();
            planetGravity.GravityMaxDistance = gravityRange;
            planetGravity.GravityFieldColor = GravityColor;
            planetGravity.LineRenderer = planet.GravityLine;

            return planetGravity;
        }

        /// <summary>
        /// Calculates movement speed and direction for a planet
        /// </summary>
        /// <param name="planet">Target planet</param>
        /// <returns></returns>
        private MoveAround InitPlanetMovement(PlanetView planet) {
            MoveAround planetMove = planet.gameObject.AddComponent<MoveAround>();
            planetMove.Speed = Random.Range(_gameConfig.PlanetMoveSpeed.Min, _gameConfig.PlanetMoveSpeed.Max);
            planetMove.Target = Sun;

            if (_gameConfig.AllowReversePlanetMove) {
                planetMove.Speed *= Mathf.Sign(Random.Range(500, -500));
            }

            return planetMove;
        }
    }
}