using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Launcher : MonoBehaviour
{
    public Button NoECSButton;
    public Button HybridECSButton;
    public Button PureECSButton;
    public Button QuitButton;

    public int NoECSSceneIndex = 1;
    public int HybridECSSceneIndex = 2;
    public int PureECSSceneIndex = 3;

    private void Start()
    {
        NoECSButton.onClick.AddListener(OpenNoECSScene);
        HybridECSButton.onClick.AddListener(OpenHybridECSScene);
        PureECSButton.onClick.AddListener(OpenNoECSScene);
        QuitButton.onClick.AddListener(Quit);
    }

    private void OpenNoECSScene() { SceneManager.LoadScene(NoECSSceneIndex); }
    private void OpenHybridECSScene() { SceneManager.LoadScene(HybridECSSceneIndex); }
    private void OpenPureECSScene() { SceneManager.LoadScene(PureECSSceneIndex); }

    private void Quit() { Application.Quit(); }
}
