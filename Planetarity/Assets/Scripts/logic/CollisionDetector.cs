using System;
using System.Collections.Generic;
using game.extensions;
using game.interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace game.logic {
    /// <summary>
    /// Detects a collision between this gameObject and some other.
    /// This gameObject should have Collider assigned with enabled parameter "Is Trigger"
    /// </summary>
    public class CollisionDetector : MonoBehaviour {
        /// <summary>
        /// With whom the object should interact
        /// </summary>
        public LayerMask CollideWith;
        /// <summary>
        /// Event to trigger on collision
        /// </summary>
        public UnityEvent OnCollisionDetected;

        private readonly List<Collider> _ignoreList = new List<Collider>();
        private Action<IDamageable> _callback;


        /// <summary>
        /// Stores a callback to call when collision with a IDamagable object detected
        /// </summary>
        /// <param name="callback"></param>
        public void SetCollisionWithDamageCallback(Action<IDamageable> callback) {
            _callback = callback;
        }

        /// <summary>
        /// Adds collider to a ignore list
        /// </summary>
        /// <param name="collider">Some collider</param>
        public void AddToIgnoreList(Collider collider) {
            _ignoreList.Add(collider);
        }

        /// <summary>
        /// Adds range of colliders to a ignore list
        /// </summary>
        /// <param name="colliders">Some colliders</param>
        public void AddRangeToIgnoreList(Collider[] colliders) {
            _ignoreList.AddRange(colliders);
        }

        private void OnTriggerEnter(Collider other) {
            // If object is within layer mask
            if (other.gameObject != null && CollideWith.Contains(other.gameObject.layer)) {
                
                // If it is a IDamageable
                IDamageable damageable = other.transform.parent.GetComponent<IDamageable>();
                if (damageable != null) {
                    _callback?.Invoke(damageable);
                }

                // Fire an event
                OnCollisionDetected?.Invoke();
            }
        }

        private void OnDestroy() {
            _callback = null;
        }
    }
}