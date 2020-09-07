using UnityEngine;
using UnityEngine.SceneManagement;

namespace game.managers {
    /// <summary>
    /// Manager to control Main Menu
    /// </summary>
    public class MenuManager : MonoBehaviour {
        private const string GAME_SCENE_NAME = "Game";
        
        public void GoToGameScene() {
            SceneManager.LoadSceneAsync(GAME_SCENE_NAME, LoadSceneMode.Single);
        }
    }
}