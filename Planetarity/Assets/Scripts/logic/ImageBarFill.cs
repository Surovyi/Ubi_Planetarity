using UnityEngine;
using UnityEngine.UI;

namespace game.logic {
    /// <summary>
    /// Component that controls fill amount of a filled type image
    /// </summary>
    public class ImageBarFill : MonoBehaviour {
        /// <summary>
        /// Target
        /// </summary>
        public Image Image;


        private float _from;
        private float _to;
        private float _current;

        private float _animationTime;
        private float _currentTime;

        private bool _shouldAnimate;


        /// <summary>
        /// Starts animation of image fill
        /// </summary>
        /// <param name="from">From value [0...1]</param>
        /// <param name="to">To value [0...1]</param>
        /// <param name="time">Animation time</param>
        public void AnimateImageFill(float from, float to, float time) {
            _animationTime = time;
            _from = from;
            _to = to;
            _currentTime = 0f;

            _shouldAnimate = true;
        }


        private void Update() {
            if (_shouldAnimate == false) {
                return;
            }

            _currentTime += Time.deltaTime;

            float t = _currentTime / _animationTime;
            float value = Mathf.Lerp(_from, _to, t);

            Image.fillAmount = value;

            if (t >= 1f) {
                _shouldAnimate = false;
            }
        }
    }
}