namespace game.interfaces {
    /// <summary>
    /// Interface to save game.
    /// NOTE: Currently there are no implementation
    /// NOTE: Potential implementations:
    /// Gather all active objects, parse their values and save:
    /// - to json
    /// - to scriptable object
    /// - to remote
    /// </summary>
    public interface ISaveGame {
        void SaveGame();
    }
}