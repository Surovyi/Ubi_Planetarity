using game.config;
using game.interfaces;
using game.managers;
using game.views;
using UnityEngine;

namespace game.input {
    /// <summary>
    /// Makes a shoot out of the gun using player input.
    /// For desktop it is a mouse click. For mobile - tap.
    /// </summary>
    public class GunShoot : MonoBehaviour {

        private PlanetView _planetView;
        private RocketConfig _rocketConfig;

        private float _lastShootDelta;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="planetView">Reference to planet view to which belongs this gun</param>
        /// <param name="rocketConfig">Configuration for this gun</param>
        /// <param name="playerControl">Does player control this gun</param>
        public void Init(PlanetView planetView, RocketConfig rocketConfig, bool playerControl) {
            _planetView = planetView;
            _rocketConfig = rocketConfig;
            _lastShootDelta = _rocketConfig.Cooldown + 1f;

            // If a player controls this gut, listen to non-ui click events
            if (playerControl) {
                InputManager.Instance.NonUiClick += Shoot;
            }
        }


        /// <summary>
        /// Shoots rocket out of the gun
        /// </summary>
        public void Shoot() {
            // Pause/Resume
            if (GameManager.AllowPlayerInput == false) {
                return;
            }

            // Check for cooldown
            if (_lastShootDelta < _rocketConfig.Cooldown) {
                return;
            }

            // Generating new rocket
            RocketView rocketView = Instantiate(_planetView.GunView.RocketPrefab,
                _planetView.GunView.RocketSpawnPoint.position,
                transform.rotation);
            rocketView.GraphicsRenderer.color = _planetView.GraphicsRenderer.color;
            rocketView.CollisionDetector.SetCollisionWithDamageCallback(OnCollisionWithDamageableObject);
            rocketView.CollisionDetector.AddToIgnoreList(_planetView.Collider);

            // Start rocket move
            Vector3 direction = Vector3.ProjectOnPlane(rocketView.transform.up, Vector3.up).normalized;
            rocketView.DestroyWithDelay.SetDelay(_rocketConfig.Lifetime);
            rocketView.MoveRocket.Init(direction, _rocketConfig);
            rocketView.MoveRocket.Move();

            // Animate cooldown in planet HUD
            _planetView.CooldownBar.AnimateImageFill(1f, 0f, _rocketConfig.Cooldown);

            _lastShootDelta = 0f;
        }

        private void OnCollisionWithDamageableObject(IDamageable damageable) {
            damageable.TakeDamage(_rocketConfig.Damage);
        }

        private void Update() {
            if (GameManager.AllowPlayerInput == false) {
                return;
            }

            _lastShootDelta += Time.deltaTime;
        }

        private void OnDestroy() {
            InputManager.Instance.NonUiClick -= Shoot;
        }
    }
}