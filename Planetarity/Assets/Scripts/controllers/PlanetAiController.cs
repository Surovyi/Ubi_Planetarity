using System.Collections.Generic;
using game.config;
using game.data;
using game.managers;
using UnityEngine;

namespace game.controllers {
    /// <summary>
    /// Basic AI assigned to planet.
    /// There are different ways to implement AI in such game.
    /// This one is not the best, but fast in development solution.
    /// </summary>
    public class PlanetAiController : MonoBehaviour {
        /// <summary>
        /// Planet controlled by this AI
        /// </summary>
        private Planet _planet;

        /// <summary>
        /// Other planets in system
        /// </summary>
        private readonly List<Planet> _otherPlanets = new List<Planet>();

        private float _nextShootTime;
        private float _nextGunRotateValue;
        private Planet _nextShootTarget;
        private AiConfig _aiConfig;
        private RocketConfig _rocketConfig;

        private bool _isActive;


        /// <summary>
        /// Initializes AI controller
        /// </summary>
        /// <param name="planet">Planet to be controlled by AI</param>
        /// <param name="otherPlanets">Any other planet</param>
        /// <param name="aiConfig">AI configuration file</param>
        /// <param name="rocketConfig">Rocket configuration file</param>
        public void Init(Planet planet, List<Planet> otherPlanets, AiConfig aiConfig, RocketConfig rocketConfig) {
            _planet = planet;
            _aiConfig = aiConfig;
            _rocketConfig = rocketConfig;

            _otherPlanets.Clear();
            _otherPlanets.AddRange(otherPlanets);
        }

        /// <summary>
        /// Activates this AI controller
        /// </summary>
        public void Activate() {
            _nextShootTime = Random.Range(_aiConfig.StartShootDelayRange.Min, _aiConfig.StartShootDelayRange.Max);
            _nextShootTarget = _otherPlanets[Random.Range(0, _otherPlanets.Count)];

            _isActive = true;
        }

        /// <summary>
        /// Makes this controller non-active
        /// </summary>
        public void Pause() {
            _isActive = false;
        }

        private void Update() {
            // Pause/Resume
            if (_isActive == false || GameManager.AllowAi == false) {
                return;
            }

            _nextShootTime -= Time.deltaTime;

            // If AI can shoot
            if (_nextShootTime < 0f && _nextShootTarget != null) {

                // Calculating target direction
                Vector3 targetPos = _nextShootTarget.PlanetView.transform.position;
                Vector3 screenDirection =
                    CameraController.CalculateScreenSpaceDirection(_planet.PlanetView.GunView.transform.position,
                        targetPos);

                // Calculating rotation to target
                float eulerAngle = Vector3.SignedAngle(Vector3.up, screenDirection, Vector3.forward);
                float random = Random.Range(_aiConfig.ShootAngleRandomCorrection.Min,
                    _aiConfig.ShootAngleRandomCorrection.Max);
                float sign = Mathf.Sign(Random.Range(-500, 500));
                eulerAngle += random * sign;

                // Rotates gun towards target
                // TODO: instead of instant rotation make a smooth one.
                _planet.PlanetView.GunView.transform.localRotation = Quaternion.Euler(0f, 0f, eulerAngle);

                // Shoots with rocket
                _planet.GunShoot.Shoot();

                Debug.Log(
                    $"[AI SHOOT]: TARGET: {_nextShootTarget.PlanetView.gameObject.name}, eulerAngle: {eulerAngle}, screenDirection: {screenDirection.normalized}");

                CalculateNextTarget();
            }
        }


        private void CalculateNextTarget() {
            _nextShootTime = _rocketConfig.Cooldown + Random.Range(_aiConfig.AdditionalShootCooldownTime.Min,
                _aiConfig.AdditionalShootCooldownTime.Max);
            _nextShootTarget = _otherPlanets[Random.Range(0, _otherPlanets.Count)];
        }
    }
}