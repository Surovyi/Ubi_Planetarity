using game.managers;
using UnityEngine;

namespace game.logic {
    /// <summary>
    /// Component that will destroy object after a specified delay value
    /// </summary>
    public class DestroyWithDelay : MonoBehaviour {
        /// <summary>
        /// Delay
        /// </summary>
        public float Delay = 1f;

        private float _destroyTimer;

        /// <summary>
        /// Sets delay
        /// </summary>
        /// <param name="delay">Delay value</param>
        public void SetDelay(float delay) {
            Delay = delay;
        }


        /// <summary>
        /// Destroy this gameObect
        /// </summary>
        public void DestroyThis() {
            Debug.Log($"[{gameObject.name}]: Destroying!");

            Destroy(gameObject);
        }
        
        private void Update() {
            // Pause/ Resume
            if (GameManager.AllowTimeUpdate == false) {
                return;
            }

            _destroyTimer += Time.deltaTime;

            if (_destroyTimer > Delay) {
                DestroyThis();
            }
        }
    }
}