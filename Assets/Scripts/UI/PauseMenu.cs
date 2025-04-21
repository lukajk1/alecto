using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject escPage;
    [SerializeField] private Button backToGame;
    [SerializeField] private Button settings;
    [SerializeField] private Button mainMenu;
    [SerializeField] private Button quit;

    private bool isOpen;

    private void Start()
    {
        backToGame.onClick.AddListener(() => SetEscMenu(false));
        settings.onClick.AddListener(() => SettingsManager.i.Open());
        mainMenu.onClick.AddListener(() => ToMainMenu());
        quit.onClick.AddListener(() => Application.Quit());
        escPage.SetActive(false);
    }

    private void Update()
    {
        // no reason to tie this to actions system, best if esc isn't rebindable.
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            SetEscMenu(!isOpen);
        }
    }
    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            if (!isOpen) SetEscMenu(true);
        }
        else
        {
            // regained focus
        }

    }
    void ToMainMenu()
    {
        SetEscMenu(false);
        Game.CursorLocked = false;
        SceneManager.LoadScene(Game.mainMenuSceneName);
    }

    private void SetEscMenu(bool value)
    {
        escPage.SetActive(value);
        isOpen = value;

        if (value)
        {
            Game.AudioListenerPaused = true;
            Game.MenusOpen++;
        }
        else
        {
            Game.AudioListenerPaused = false;
            Game.MenusOpen--;
        }
    }
}
