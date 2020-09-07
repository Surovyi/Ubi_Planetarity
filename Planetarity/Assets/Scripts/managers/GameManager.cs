using System.Collections;
using System.Collections.Generic;
using game.config;
using game.controllers;
using game.data;
using game.input;
using game.views;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace game.managers {
    /// <summary>
    /// Gameplay manager class.
    /// Responsible for establishing and maintaining player gameplay experience.
    /// </summary>
    public class GameManager : MonoBehaviour {
        private const float GO_TO_MENU_DELAY = 3f;
        private const string MENU_SCENE_NAME = "Menu";
        
        private static GameManager sInstance;

        /// <summary>
        /// Allows movement to celesial objects. Pause/ Resume.
        /// </summary>
        public static bool AllowCelestialsMovement => sInstance._allowCelestialsMovement;
        /// <summary>
        /// Allows player input. Pause/ Resume.
        /// </summary>
        public static bool AllowPlayerInput => sInstance._allowPlayerInput;
        /// <summary>
        /// Allows time to update. Pause/ Resume.
        /// </summary>
        public static bool AllowTimeUpdate => sInstance._allowTimeUpdate;
        /// <summary>
        /// Allows camera to move. Pause/ Resume.
        /// </summary>
        public static bool AllowCameraMovement => sInstance._allowCameraMovement;
        /// <summary>
        /// Allows AI to update its behaviour. Pause/ Resume.
        /// </summary>
        public static bool AllowAi => sInstance._allowAi;

        /// <summary>
        /// Reference to main game configuration asset
        /// </summary>
        public TextAsset GameConfig;
        /// <summary>
        /// Reference to ai configuration asset
        /// </summary>
        public TextAsset AiConfig;
        /// <summary>
        /// Reference to rocket configuration assets
        /// </summary>
        public TextAsset[] RocketConfigs;
        /// <summary>
        /// Reference to spawn manager
        /// </summary>
        public SpawnManager SpawnManager;
        /// <summary>
        /// Reference to camera controller
        /// </summary>
        public CameraController CameraController;
        /// <summary>
        /// Reference to Sun in scene
        /// </summary>
        public SunView Sun;
        /// <summary>
        /// Reference to UI
        /// </summary>
        public UiView UiView;

        private bool _allowCelestialsMovement = true;
        private bool _allowPlayerInput = true;
        private bool _allowTimeUpdate = true;
        private bool _allowCameraMovement = true;
        private bool _allowAi = true;

        private GameConfig _gameConfig;
        private AiConfig _aiConfig;
        private CelestialObject[] _celestialObjects;
        private readonly List<Planet> _planets = new List<Planet>();


        /// <summary>
        /// Calculates gravity for a specified world position and object mass.
        /// F = G x m1xm2/r*r;
        /// </summary>
        /// <param name="objectPosition">Object world position</param>
        /// <param name="objectMass">Object mass</param>
        /// <returns></returns>
        public static Gravity CalculateGravityForce(Vector3 objectPosition, float objectMass) {
            Gravity gravity = new Gravity();

            // If there is not a single celestial object - there is no gravity
            if (sInstance._celestialObjects == null) {
                return gravity;
            }

            Vector3 resultGravityForce = Vector3.zero;

            // For each object calculate gravity direction and force
            foreach (CelestialObject celestial in sInstance._celestialObjects) {
                Vector3 forceDirection = (celestial.Transform.position - objectPosition).normalized;
                float distance = (celestial.Transform.position - objectPosition).sqrMagnitude;
                float gravityForce = sInstance._gameConfig.UniverseGravitationalConstant * objectMass * celestial.Mass /
                                     distance;

                resultGravityForce += forceDirection * gravityForce;
            }

            // Store and return calculated values
            gravity.Direction = resultGravityForce.normalized;
            gravity.Force = sInstance._gameConfig.ClampMaxGravity
                ? Mathf.Clamp(resultGravityForce.magnitude, 0f, sInstance._gameConfig.MaxGravityForce)
                : resultGravityForce.magnitude;

            return gravity;
        }

        /// <summary>
        /// Explicitly tell that one of the planet is run out of HP.
        /// </summary>
        /// <param name="planet">Planet without HP</param>
        public static void PlanetRunsOutOfHp(Planet planet) {
            planet.Alive = false;

            // If it was player planet - we loss. Otherwise - we win.
            if (planet.UnderPlayerControl == true) {
                sInstance.UiView.LossRoot.SetActive(true);
                sInstance.UiView.PauseResumeRoot.SetActive(false);
                sInstance.PauseGame();
                sInstance.StartCoroutine(sInstance.GoToMenu());
            }
            else {
                List<Planet> alive = sInstance._planets.FindAll(x => x.Alive == true);
                if (alive.Count == 1 && alive[0].UnderPlayerControl) {
                    sInstance.UiView.WinRoot.SetActive(true);
                    sInstance.UiView.PauseResumeRoot.SetActive(false);
                    sInstance.PauseGame();
                    sInstance.StartCoroutine(sInstance.GoToMenu());
                }
            }

        }

        /// <summary>
        /// Pauses the game
        /// </summary>
        public void PauseGame() {
            _allowCelestialsMovement = false;
            _allowPlayerInput = false;
            _allowTimeUpdate = false;
            _allowCameraMovement = false;
            _allowAi = false;
        }

        /// <summary>
        /// Resumes the game
        /// </summary>
        public void ResumeGame() {
            _allowCelestialsMovement = true;
            _allowPlayerInput = true;
            _allowTimeUpdate = true;
            _allowCameraMovement = true;
            _allowAi = true;
        }

        /// <summary>
        /// Return to menu in a short time
        /// </summary>
        /// <returns></returns>
        private IEnumerator GoToMenu() {
            yield return new WaitForSeconds(GO_TO_MENU_DELAY);

            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }

        
        private void Awake() {
            // Singleton (simple version)
            if (sInstance != null) {
                Assert.IsNull(sInstance, "GameManager.Awake => sInstance != null");
                Destroy(gameObject);
                return;
            }

            sInstance = this;

            // Ensure that all references set in Inspector
            Assert.IsNotNull(GameConfig, "GameManager.Awake => GameConfig == null");
            Assert.IsNotNull(AiConfig, "GameManager.Awake => AiConfig == null");
            Assert.IsNotNull(RocketConfigs, "GameManager.Awake => RocketConfigs == null");
            Assert.IsNotNull(SpawnManager, "GameManager.Awake => SpawnManager == null");
            Assert.IsNotNull(CameraController, "GameManager.Awake => CameraController == null");
            Assert.IsNotNull(Sun, "GameManager.Awake => Sun == null");

            // Read game config
            if (_gameConfig == null) {
                Debug.Log($"GameManager.Awake => Parse GameConfig JSON: \n{GameConfig.text}\n");
                _gameConfig = JsonUtility.FromJson<GameConfig>(GameConfig.text);
            }

            // Read ai config
            if (_aiConfig == null) {
                Debug.Log($"GameManager.Awake => Parse AiConfig JSON: \n{AiConfig.text}\n");
                _aiConfig = JsonUtility.FromJson<AiConfig>(AiConfig.text);
            }

            //Spawn planets
            _planets.AddRange(SpawnManager.SpawnPlanets(_gameConfig));
            SetupCelestialObjectArray();

            //Make planets move
            StartPlanetMovement();

            //Give player control on one of the planets
            AssignPlanetToPlayerControl(_planets[Random.Range(0, _planets.Count)]);

            //Give AI control on other planers
            for (int i = 0; i < _planets.Count; i++) {
                if (_planets[i].UnderPlayerControl == true) {
                    continue;
                }

                AssignPlanetToAiControl(_planets[i], i);
                CameraController.AddObjectToBeInView(_planets[i].PlanetView.transform);
            }

            //Make camera follow player planet
            CameraController.SetCameraSizeBounds(_gameConfig.CameraSizeRange.Min, _gameConfig.CameraSizeRange.Max);
        }


        private void StartPlanetMovement() {
            foreach (Planet planet in _planets) {
                planet.MoveAround.Move();
            }
        }

        private void StopPlanetMovement() {
            foreach (Planet planet in _planets) {
                planet.MoveAround.Stop();
            }
        }

        private void AssignPlanetToPlayerControl(Planet planet) {
            // Generate gun rotate
            GunRotate gunRotate = planet.PlanetView.GunView.GunTransform.gameObject.AddComponent<GunRotate>();
            planet.PlanetView.gameObject.name = "[PLAYER] planet";
            planet.GunRotate = gunRotate;
            planet.UnderPlayerControl = true;

            CameraController.AddObjectToBeInView(planet.PlanetView.transform);

            string randomRocketJson = RocketConfigs[Random.Range(0, RocketConfigs.Length)].text;
            RocketConfig randomRocketTypeConfig = JsonUtility.FromJson<RocketConfig>(randomRocketJson);

            // Generate gun shoot
            GunShoot gunShoot = planet.PlanetView.GunView.GunTransform.gameObject.AddComponent<GunShoot>();
            gunShoot.Init(planet.PlanetView, randomRocketTypeConfig, true);

            planet.GunShoot = gunShoot;
        }

        private void AssignPlanetToAiControl(Planet planet, int i) {
            planet.PlanetView.gameObject.name = $"[AI {i}] planet";
            planet.PlanetView.PlayerNickname.text = $"[AI {i}]";

            // Generate AI Controller
            PlanetAiController ai = planet.PlanetView.gameObject.AddComponent<PlanetAiController>();
            List<Planet> otherPlanets = new List<Planet>();
            otherPlanets.AddRange(_planets);
            otherPlanets.Remove(planet);

            string randomRocketJson = RocketConfigs[Random.Range(0, RocketConfigs.Length)].text;
            RocketConfig randomRocketTypeConfig = JsonUtility.FromJson<RocketConfig>(randomRocketJson);

            // Generate gun shoot
            GunShoot gunShoot = planet.PlanetView.GunView.GunTransform.gameObject.AddComponent<GunShoot>();
            gunShoot.Init(planet.PlanetView, randomRocketTypeConfig, false);
            planet.GunShoot = gunShoot;

            ai.Init(planet, otherPlanets, _aiConfig, randomRocketTypeConfig);
            ai.Activate();
        }


        private void SetupCelestialObjectArray() {
            _celestialObjects = new CelestialObject[_planets.Count + 1];
            for (int i = 0; i < _planets.Count; i++) {
                _celestialObjects[i] = _planets[i];
                _celestialObjects[i].Transform = _planets[i].PlanetView.transform;
            }

            Sun sun = new Sun {
                SunView = Sun,
                Mass = _gameConfig.SunMass,
                Transform = Sun.transform
            };
            _celestialObjects[_celestialObjects.Length - 1] = sun;
        }

        private void OnDestroy() {
            sInstance = null;
        }
    }
}