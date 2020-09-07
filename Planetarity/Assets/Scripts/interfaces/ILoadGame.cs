namespace game.interfaces {
    /// <summary>
    /// Interface to load game.
    /// NOTE: Currently there are no implementation
    /// NOTE: Potential implementations:
    /// - from json
    /// - from scriptable object
    /// - from remote
    /// </summary>
    public interface ILoadGame {
        void LoadGame();
    }
}