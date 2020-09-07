using System;

namespace game.extensions {
    /// <summary>
    /// Data structure to store a tuple of integers
    /// </summary>
    [Serializable]
    public struct MinMaxInt {
        public int Min;
        public int Max;
    }

    /// <summary>
    /// Data structure to store a tuple of floats
    /// </summary>
    [Serializable]
    public struct MinMaxFloat {
        public float Min;
        public float Max;
    }
}