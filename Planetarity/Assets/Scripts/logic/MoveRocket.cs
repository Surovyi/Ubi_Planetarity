using game.config;
using game.controllers;
using game.data;
using game.managers;
using UnityEngine;

namespace game.logic {

    /// <summary>
    /// Moves rocket after it is being shoot.
    /// </summary>
    public class MoveRocket : MonoBehaviour {
        private Vector3 _direction;
        private RocketConfig _rocketConfig;
        private bool _isMoving;


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="initialDirection">Directions to move</param>
        /// <param name="rocketConfig">Configuration asset</param>
        public void Init(Vector3 initialDirection, RocketConfig rocketConfig) {
            _rocketConfig = rocketConfig;
            _direction = initialDirection;
        }

        /// <summary>
        /// Rocket will move
        /// </summary>
        public void Move() {
            _isMoving = true;
        }

        /// <summary>
        /// Rocket will stop
        /// </summary>
        public void Stop() {
            _isMoving = false;
        }


        private void Update() {
            // Pause/Resume
            if (_isMoving == false || GameManager.AllowCelestialsMovement == false) {
                return;
            }

            // Calculates new rocket position and rotation using gravity
            Vector3 currentPosition = transform.position;
            Vector3 newPosition = CalculateNewRocketPosition(currentPosition);
            Quaternion newRotation = CalculateNewRocketRotation(currentPosition, newPosition);

            _direction = (newPosition - currentPosition).normalized;

            // Applying position and rotation
            transform.position = newPosition;
            transform.localRotation = newRotation;
        }


        private Vector3 CalculateNewRocketPosition(Vector3 currentPosition) {
            Gravity gravityData = GameManager.CalculateGravityForce(currentPosition, _rocketConfig.Mass);

            Vector3 gravity = gravityData.Force * gravityData.Direction;
            Vector3 moveForce = _direction * (Time.deltaTime * _rocketConfig.InitialAccelerationMultiplier);
            Vector3 nextNonGravityPos = currentPosition + moveForce;
            Vector3 newPosition = nextNonGravityPos + gravity;

            return newPosition;
        }

        private Quaternion CalculateNewRocketRotation(Vector3 currentPosition, Vector3 newPosition) {
            Vector3 screenDirection = CameraController.CalculateScreenSpaceDirection(currentPosition, newPosition);
            float eulerAngle = Vector3.SignedAngle(Vector3.up, screenDirection, Vector3.forward);

            Quaternion newRotation = Quaternion.Euler(45f, -45f, eulerAngle);
            return newRotation;
        }
    }
}