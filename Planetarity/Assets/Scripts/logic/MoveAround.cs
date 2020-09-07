using game.managers;
using UnityEngine;
using UnityEngine.Assertions;

namespace game.logic {
    /// <summary>
    /// Component that moves object around target.
    /// </summary>
    public class MoveAround : MonoBehaviour {
        [Tooltip("The target around which this object will rotate")]
        public Transform Target;

        [Tooltip("Controls moving velocity")] public float Speed = 1f;

        [Tooltip("Should this component listen to pause/resume")]
        public bool UseGlobalRestrictions = true;

        private float _elapsedTime;
        private float _distanceMagnitude;
        private bool _isMoving;

        
        /// <summary>
        /// Calculates orbit position using time and distance values
        /// </summary>
        /// <param name="time">Time value</param>
        /// <param name="distance">Distance value</param>
        /// <returns></returns>
        public static Vector3 GetOrbitPosition(float time, float distance) {
            float x = Mathf.Sin(time) * distance;
            float z = Mathf.Cos(time) * distance;

            return new Vector3(x, 0f, z);
        }

        /// <summary>
        /// Calculates next frame orbit position for specified object that moves around orbit
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Vector3 GetNextOrbitPosition(MoveAround obj) {
            return GetOrbitPosition(obj._elapsedTime + Time.fixedDeltaTime * obj.Speed / obj._distanceMagnitude,
                obj._distanceMagnitude);
        }
        
        

        /// <summary>
        /// Start movement
        /// </summary>
        public void Move() {
            _isMoving = true;

            Assert.IsNotNull(Target, "MoveAround.Move => Target != null");

            Vector3 distanceToTarget = transform.position - Target.position;
            _distanceMagnitude = distanceToTarget.magnitude;

            float angle = Vector3.SignedAngle(Vector3.forward, distanceToTarget.normalized, Vector3.up);
            _elapsedTime = angle * Mathf.Deg2Rad;
        }

        /// <summary>
        /// Stop movement
        /// </summary>
        public void Stop() {
            _isMoving = false;
        }

        private void Awake() {
            if (Target != null) {
                Move();
            }
        }


        private void FixedUpdate() {
            // Pause/ Resume
            if (_isMoving == false || UseGlobalRestrictions && GameManager.AllowCelestialsMovement == false) {
                return;
            }

            _elapsedTime += Time.fixedDeltaTime * Speed / _distanceMagnitude;

            Vector3 newPosition = CalculateTargetLocalPosition();

            transform.localPosition = newPosition;
        }

        private Vector3 CalculateTargetLocalPosition() {
            return GetOrbitPosition(_elapsedTime, _distanceMagnitude);
        }
    }
}