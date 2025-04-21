using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static bool isPaused;
    public static bool IsPaused 
    { 
        get
        {
            return isPaused;
        }
        private set
        {
            if (value != isPaused)
            {
                isPaused = value;
                PauseUpdated?.Invoke(value);

                if (value)
                {
                    Time.timeScale = 0;
                    CursorLocked = false;
                }
                else
                {
                    Time.timeScale = 1;
                    CursorLocked = true;
                }
            }
        }
    }
    private static bool isInDialogue;
    public static bool IsInDialogue { get; set; }

    private static int _menusOpen;
    public static int MenusOpen
    {
        get => _menusOpen;
        set
        {
            _menusOpen = value;
            if (_menusOpen == 0)
            {
                IsPaused = false;
            }
            else
            {
                IsPaused = true;
            }
        }
    }

    private static bool _cursorLocked = false;
    public static bool CursorLocked
    {
        get => _cursorLocked;
        set
        {
            if (_cursorLocked != value)
            {
                _cursorLocked = value;
            }

            if (_cursorLocked) Cursor.lockState = CursorLockMode.Locked;
            else Cursor.lockState = CursorLockMode.None;
        }
    }

    private static bool _audioListenerPaused = false;
    public static bool AudioListenerPaused
    {
        get => _audioListenerPaused;
        set
        {
            if (_audioListenerPaused != value)
            {
                _audioListenerPaused = value;
            }

            if (_audioListenerPaused) AudioListener.pause = true;
            else AudioListener.pause = false;
        }
    }

    private static float _timeScale = 1f;
    public static float TimeScale
    {
        get => _timeScale;
        set
        {
            if (_timeScale != value)
            {
                // updating only when the value changes prevents repeatededly invoking events if I hook on up here later
                _timeScale = value;
                Time.timeScale = _timeScale;
            }
        }
    }

    public static int _score = 0;
    public static int Score
    {
        get => _score;
        set
        {
            _score = value;
            ScoreUpdated?.Invoke();
        }
    }

    public Transform PlayerTransform;
    public Camera PlayerCamera;
    public PlayerUnit PlayerUnitInstance;
    public CapsuleCollider PlayerBodyCollider;
    public SphereCollider PlayerHeadCollider;

    public static Action<bool> PauseUpdated;
    public static Action ScoreUpdated;
    public static Action InitializeRun;
    public static Game i { get; private set; }

    public static string mainMenuSceneName = "MainMenu";
    public static string mainGameSceneName = "Game";
    public static string tutorialSceneName = "Tutorial";

    private void Awake()
    {
        if (i != null) Debug.LogError($"More than one instance of {i} in scene");
        i = this;
    }

    private void Start()
    {
        InitializeRun?.Invoke();
        CursorLocked = true;
        PlayerUnitInstance = FindFirstObjectByType<PlayerUnit>();
        PlayerTransform = GameObject.Find("Player").transform;
    }
}
