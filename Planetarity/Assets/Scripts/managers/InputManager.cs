using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace game.managers {
    /// <summary>
    /// Controls user input.
    /// Very simple implementation.
    /// TODO: remove deriving from monobehaviour
    /// </summary>
    public class InputManager : MonoBehaviour {
        /// <summary>
        /// Singleton
        /// </summary>
        public static InputManager Instance {
            get {
                if (sInstance == null) {
                    sInstance = FindObjectOfType<InputManager>();
                    if (sInstance == null) {
                        GameObject inputManager = new GameObject("_InputManager");
                        sInstance = inputManager.AddComponent<InputManager>();
                    }
                }

                return sInstance;
            }
        }

        private static InputManager sInstance;

        public event Action NonUiClick;


        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                // Prevents click events if they were made on UI
                if (EventSystem.current.IsPointerOverGameObject()) {
                    return;
                }

                NonUiClick?.Invoke();
            }
        }

        private void OnDestroy() {
            sInstance = null;
        }
    }
}