using UnityEngine;
using UnityEngine.Assertions;

namespace game.logic.draw {
    /// <summary>
    /// Draws planet orbit using specified values
    /// </summary>
    [DisallowMultipleComponent]
    public class DrawOrbit : MonoBehaviour {
        /// <summary>
        /// Orbit will be made relative to this target object
        /// </summary>
        public Transform RelativeTo;
        /// <summary>
        /// Renderer to draw orbit with
        /// </summary>
        public LineRenderer LineRenderer;
        /// <summary>
        /// Default orbit color
        /// </summary>
        public Color OrbitColor = Color.yellow;
        /// <summary>
        /// Number of steps to draw orbit
        /// </summary>
        [Range(4, 100)] public int Steps = 50;

        private float _distanceMagnitude;

        private void Start() {
            // Ensure that references specified
            Assert.IsNotNull(RelativeTo, "DrawOrbit.Start => RelativeTo != null");
            Assert.IsNotNull(LineRenderer, "DrawOrbit.Start => LineRenderer != null");

            Vector3 distanceToTarget = transform.position - RelativeTo.position;
            _distanceMagnitude = distanceToTarget.magnitude;

            DrawOrbitLines();
        }

        /// <summary>
        /// Draws planet orbit.
        /// Used once on level start, but this operation may have performance cost.
        /// Solution is to use separate thread for calculation.
        /// </summary>
        private void DrawOrbitLines() {
            // Calculate line points
            float stepValue = 2 * Mathf.PI / (Steps - 1);
            Vector3[] positions = new Vector3[Steps];

            for (int i = 0; i < Steps; i++) {
                float elapsedTime = stepValue * i;

                Vector3 point = MoveAround.GetOrbitPosition(elapsedTime, _distanceMagnitude);
                positions[i] = point;
            }

            // Assign values and make renderer visible
            Color color = OrbitColor;
            color.a = 0.5f;
            LineRenderer.startColor = color;
            LineRenderer.endColor = color;
            LineRenderer.positionCount = Steps;
            LineRenderer.SetPositions(positions);
        }
    }
}