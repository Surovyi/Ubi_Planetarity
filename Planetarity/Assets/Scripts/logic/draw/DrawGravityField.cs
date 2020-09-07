using UnityEngine;
using UnityEngine.Assertions;

namespace game.logic.draw {
    /// <summary>
    /// Draws gravity field using specified values
    /// </summary>
    [DisallowMultipleComponent]
    public class DrawGravityField : MonoBehaviour {
        public LineRenderer LineRenderer;
        public Color GravityFieldColor = Color.gray;
        [Range(0.1f, 10f)] public float GravityMaxDistance = 1f;
        [Range(4, 100)] public int Steps = 50;
    
    
        private void Start() {
            // Ensure that renderer is specified
            Assert.IsNotNull(LineRenderer, "DrawGravityField.Start => LineRenderer != null");

            DrawGravityFieldLines();
        }
    
        /// <summary>
        /// Draws gravity field.
        /// Used once on level start, but this operation may have performance cost.
        /// Solution is to use separate thread for calculation.
        /// </summary>
        private void DrawGravityFieldLines() {
            // Calculate line points
            float stepValue = 2 * Mathf.PI / (Steps - 1);
            Vector3[] positions = new Vector3[Steps];
        
            for (int i = 0; i < Steps; i++) {
                float elapsedTime = stepValue * i;
            
                Vector3 point = MoveAround.GetOrbitPosition(elapsedTime, GravityMaxDistance);
                positions[i] = point;
            }
        
            // Assign values and make renderer visible
            Color color = GravityFieldColor;
            color.a = 0.5f;
            LineRenderer.startColor = color;
            LineRenderer.endColor = color;
            LineRenderer.positionCount = Steps;
            LineRenderer.SetPositions(positions);
        }
    }
}