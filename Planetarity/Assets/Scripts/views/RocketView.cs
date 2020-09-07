using game.logic;

namespace game.views {
    /// <summary>
    /// View for a rocket spawned by planet
    /// </summary>
    public class RocketView : CelectialObjectView {
        public MoveRocket MoveRocket;
        public CollisionDetector CollisionDetector;
        public DestroyWithDelay DestroyWithDelay;
    }
}