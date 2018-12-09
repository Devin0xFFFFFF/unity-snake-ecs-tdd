using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.NoECS
{
    public class Quit : MonoBehaviour
    {
        public Button QuitButton;

        private void Start() { QuitButton.onClick.AddListener(OnQuit); }

        private void OnQuit() { SceneManager.LoadScene(0); }
    }
}
