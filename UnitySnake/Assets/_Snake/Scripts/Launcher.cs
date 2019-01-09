using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UnitySnake
{
    public class Launcher : MonoBehaviour
    {
        public Button PlayButton;
        public Button QuitButton;

        public int GameSceneIndex = 1;

        private void Start()
        {
            PlayButton.onClick.AddListener(OpenNoECSScene);
            QuitButton.onClick.AddListener(Quit);
        }

        private void OpenNoECSScene() { SceneManager.LoadScene(GameSceneIndex); }

        private void Quit() { Application.Quit(); }
    }
}
