using UnityEngine;

namespace game.views {
    /// <summary>
    /// View for a planet gun
    /// </summary>
    public class GunView : MonoBehaviour {
        public Transform GunTransform;
        public SpriteRenderer GunRenderer;
        public Transform RocketSpawnPoint;
        public RocketView RocketPrefab;
    }
}