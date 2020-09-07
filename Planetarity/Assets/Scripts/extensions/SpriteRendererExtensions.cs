using UnityEngine;

namespace game.extensions {
    /// <summary>
    /// Extensions to add a new handful functionality to already existing classes
    /// </summary>
    public static class SpriteRendererExtensions {
        /// <summary>
        /// Generates random color from specified hue range and assigns it to the SpriteRenderer color component
        /// </summary>
        /// <param name="renderer">SpriteRenderer component</param>
        /// <param name="minHue">Min hue value</param>
        /// <param name="maxHue">Max hue value</param>
        public static void SetRandomColor(this SpriteRenderer renderer, int minHue = 0, int maxHue = 360) {
            float newHue = Random.Range(minHue, maxHue) / 360f;

            Color.RGBToHSV(renderer.color, out _, out float s, out float v);
            renderer.color = Color.HSVToRGB(newHue, s, v);
        }
    }
}