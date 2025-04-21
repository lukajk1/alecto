using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Console : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;

    private PlayerLookAndMove playerController;
    private GameObject inputParent;

    private string lastCommand;

    private Vector3 playerInitialPosition;
    private GameObject player;

    void Start()
    {
        playerController = FindFirstObjectByType<PlayerLookAndMove>();

        player = GameObject.Find("Player"); //replace in future when I can find by a unique type 
        playerInitialPosition = player.transform.position; 

        inputParent = inputField.transform.parent.gameObject;
        inputParent.SetActive(false);
        inputField.onEndEdit.AddListener(OnSubmit);
    }

    void Update()
    {
        HandleKeyboardCommands();
    }

    private bool HandleCommand(string commandArg)
    {
        string formatted = commandArg.Trim().ToLower();
        string[] parsed = SplitCommand(formatted);

        if (parsed.Length == 0)
        {
            Debug.LogWarning("Command cannot be empty.");
            return false;
        }
        if (parsed[0] == "printstats")
        {
            FindFirstObjectByType<RunStatsManager>().PrintStats();
            return true;
        }
        if (parsed[0] == "die")
        {
            Game.i.PlayerUnitInstance.KillViaCommand();
            return true;
        }


        if (parsed[0] == "s" && parsed.Length == 2)
        {
            return WaveManager.i.CommandSpawnEnemy(parsed[1]);
        }

        if (parsed[0] == "player" || parsed[0] == "p")
        {
            if (parsed[1] == "speed" || parsed[1] == "s")
            {
                if (float.TryParse(parsed[2], out float speed))
                {
                    playerController.MoveSpeed = speed;
                    return true;
                }
                else
                {
                    Debug.LogWarning($"Invalid value for player speed: {parsed[2]}");
                    return false;
                }
            }
            else if (parsed[1] == "jump" || parsed[1] == "j")
            {
                if (float.TryParse(parsed[2], out float jumpForce))
                {
                    playerController.JumpForce = jumpForce;
                    return true;
                }
                else
                {
                    Debug.LogWarning($"Invalid value for player jump: {parsed[2]}");
                    return false;
                }
            }
            else if ((parsed[1] == "values" || parsed[1] == "v") && (parsed[2] == "reset" || parsed[2] == "r"))
            {
                playerController.ResetValues();
                return true;
            }
        }

        else if (parsed[0] == "set" || parsed[0] == "s")
        {
            if (parsed[1] == "weather" || parsed[1] == "w")
            {
                return WeatherController.Instance.SetWeatherFromString(parsed[2]);
            }
            else if (parsed[1] == "tracking")
            {
                //Inventory.Instance.SetWeapon(1, )
            }
        }

        else if (parsed[0] == "reset" || parsed[0] == "r")
        {
            player.transform.position = playerInitialPosition;
            return true;
        }

        else if (parsed[0] == "fov")
        {
            if (float.TryParse(parsed[1], out float fov))
            {
                FindAnyObjectByType<FOVManager>().FOV = fov;
                return true;
            }
        }

        // otherwise
        Debug.LogWarning($"Unknown command: {formatted}");
        return false;
    }


    private void HandleKeyboardCommands()
    {
        if (inputParent.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                inputParent.SetActive(false);
                Game.MenusOpen--;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                inputField.text = lastCommand;
                inputField.caretPosition = inputField.text.Length;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Slash))
        {
            Game.MenusOpen++;
            inputParent.SetActive(true);
            inputField.ActivateInputField();
        }
    }

    private void OnSubmit(string command)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (HandleCommand(command))
            {
                lastCommand = command;
            }

            inputField.text = ""; // Clear the input field
            inputParent.SetActive(false);
            Game.MenusOpen--;
        }
    }

    private string[] SplitCommand(string command)
    {
        return command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    }

}