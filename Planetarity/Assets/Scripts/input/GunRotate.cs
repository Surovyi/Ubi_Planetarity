using game.controllers;
using game.managers;
using UnityEngine;

namespace game.input {
    /// <summary>
    /// Rotates gun towards player cursor position.
    /// TODO: rewrite to add support for mobile devices. 
    /// </summary>
    public class GunRotate : MonoBehaviour {

        private void Update() {
            if (GameManager.AllowPlayerInput == false) {
                return;
            }
        
            // Calculating new rotations value
            Vector3 screenDirection = CameraController.CalculateDirectionToInputPosition(transform.position);
            float eulerAngle = Vector3.SignedAngle(Vector3.up, screenDirection, Vector3.forward);
        
            // Assigning new rotation
            transform.localRotation = Quaternion.Euler(0f, 0f, eulerAngle);
        }
    }
}

