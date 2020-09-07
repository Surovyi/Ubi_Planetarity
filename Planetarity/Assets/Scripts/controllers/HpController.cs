using game.data;
using game.interfaces;
using game.logic;
using game.managers;
using UnityEngine;

namespace game.controllers {
    /// <summary>
    /// Controls a planet HP
    /// </summary>
    public class HpController : MonoBehaviour, IDamageable {
        /// <summary>
        /// Reference to the HP bar image with "filled" image type
        /// </summary>
        public ImageBarFill HpBar;

        private int _maxHp;
        private int _hp;
        private Planet _planet;

        /// <summary>
        /// Initialize controller
        /// </summary>
        /// <param name="maxHp">Max planet HP</param>
        /// <param name="planet">Reference to the planet this controller is assigned to</param>
        public void Init(int maxHp, Planet planet) {
            _maxHp = maxHp;
            _hp = maxHp;
            _planet = planet;
        }

        /// <summary>
        /// Applies damage
        /// </summary>
        /// <param name="damage">Amount of damage</param>
        public void TakeDamage(int damage) {
            float prevHp = (float) _hp / _maxHp;
            _hp -= damage;
            float newHp = (float) _hp / _maxHp;

            // Animating damage take
            HpBar.AnimateImageFill(prevHp, newHp, 0.5f);

            Debug.Log($"Damage applied! New _hp: {_hp}");

            // If 0 HP left, tells game system about this event
            if (_hp <= 0) {
                GameManager.PlanetRunsOutOfHp(_planet);
            }
        }
    }
}