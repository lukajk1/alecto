using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button tutorial;
    [SerializeField] private Button mainGame;
    [SerializeField] private Button settings;
    [SerializeField] private Button quit; 
    
    private void Start()
    {
        mainGame.onClick.AddListener(() => SceneManager.LoadScene(Game.mainGameSceneName));
        tutorial.onClick.AddListener(() => SceneManager.LoadScene(Game.tutorialSceneName));
        settings.onClick.AddListener(() => FindAnyObjectByType<SettingsManager>().Open());
        quit.onClick.AddListener(() => Application.Quit());
        
    }
}
