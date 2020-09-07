using System;
using System.Collections.Generic;
using game.managers;
using UnityEngine;
using UnityEngine.Assertions;

namespace game.controllers {
    /// <summary>
    /// Controls camera movement and size.
    /// </summary>
    public class CameraController : MonoBehaviour {
        private static CameraController sInstance;

        /// <summary>
        /// Target camera
        /// </summary>
        public Camera Camera;

        /// <summary>
        /// Objects which camera should keep in its view 
        /// </summary>
        public List<Transform> ObjectsToBeInView = new List<Transform>();

        private Transform _cameraTransform;
        private FocusType _focusType;
        private readonly List<Transform> _objectsInFocus = new List<Transform>();

        private Vector2 _cameraSizeBounds;
        private Vector3 _initialCameraOffset;
        private float _initialForwardMagnitude;


        /// <summary>
        /// Calculates screen-space direction from two world positions
        /// </summary>
        /// <param name="fromWorld">First world point</param>
        /// <param name="toWorld">Second world point</param>
        /// <returns>Screen-space direction</returns>
        public static Vector3 CalculateScreenSpaceDirection(Vector3 fromWorld, Vector3 toWorld) {
            Vector3 screenCurrentPos = sInstance.Camera.WorldToScreenPoint(fromWorld);
            Vector3 screenNewPos = sInstance.Camera.WorldToScreenPoint(toWorld);

            return screenNewPos - screenCurrentPos;
        }

        /// <summary>
        /// Calculates direction from world position to mouse cursor
        /// </summary>
        /// <param name="fromWorld">World point</param>
        /// <returns>Screen-space direction to mouse cursor</returns>
        public static Vector3 CalculateDirectionToInputPosition(Vector3 fromWorld) {
            Vector3 screenCurrentPos = sInstance.Camera.WorldToScreenPoint(fromWorld);
            Vector3 screenNewPos = Input.mousePosition;

            return screenNewPos - screenCurrentPos;
        }

        /// <summary>
        /// Calculates screen-space position of a point
        /// </summary>
        /// <param name="point">Target point</param>
        /// <returns>Screen-space point position</returns>
        public static Vector3 GetScreenSpacePosition(Vector3 point) {
            return sInstance.Camera.WorldToScreenPoint(point);
        }

        /// <summary>
        /// Saves camera size boundaries
        /// </summary>
        /// <param name="min">Min orthographic size</param>
        /// <param name="max">Max orthographic size</param>
        public void SetCameraSizeBounds(float min, float max) {
            _cameraSizeBounds = new Vector2(min, max);
        }

        /// <summary>
        /// Gives camera instructions to keep target object in sight
        /// </summary>
        /// <param name="target">Target object</param>
        public void AddObjectToBeInView(Transform target) {
            _objectsInFocus.Add(target);
        }

        /// <summary>
        /// Removes object from "in sight" list
        /// </summary>
        /// <param name="target">Target object</param>
        public void RemoveObjectFromView(Transform target) {
            _objectsInFocus.Remove(target);
        }


        private void Awake() {
            // Singleton (very simple)
            if (sInstance != null) {
                // Ensure that we have only one CameraController object
                Assert.IsNull(sInstance, "CameraController.Awake => sInstance != null");
                Destroy(gameObject);
                return;
            }

            sInstance = this;

            // Ensure that reference to camera is assigned in inspector
            Assert.IsNotNull(Camera, "CameraController.Awake => Camera != null");

            _cameraTransform = Camera.transform;
            _initialCameraOffset = _cameraTransform.forward * _cameraTransform.position.magnitude;

            if (ObjectsToBeInView?.Count > 0) {
                _objectsInFocus.AddRange(ObjectsToBeInView);
            }
        }

        private void LateUpdate() {
            // Pause/Resume
            if (GameManager.AllowCameraMovement == false) {
                return;
            }

            // Leave function if nothing specified as "to be in sight"
            if (_objectsInFocus.Count <= 0) {
                return;
            }

            // Calculating middle point of all "objects in sight" 
            Vector3 targetLocation = Vector3.zero;
            foreach (Transform obj in _objectsInFocus) {
                targetLocation += obj.position;
            }

            targetLocation /= _objectsInFocus.Count;

            // Smooth camera translation to new location
            Vector3 targetPos = targetLocation - _initialCameraOffset;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime);

            // Specifying new camera size to cover all objects that should be in sight
            // TODO: currently, on wide-screen devices objects may be slightly out of sight. Fix this.
            float targetCameraSize = Mathf.Clamp(targetLocation.magnitude, _cameraSizeBounds.x, _cameraSizeBounds.y);
            float cameraSize = Mathf.Lerp(Camera.orthographicSize, targetCameraSize, Time.deltaTime);
            Camera.orthographicSize = cameraSize;
        }


        private void OnDestroy() {
            sInstance = null;
        }
    }
}